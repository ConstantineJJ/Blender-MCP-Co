---
name: Blender Organic Sculpting
version: 1.0
description: Deterministic-first organic sculpting skill for Blender characters. Uses bounded region deformation, smoothing, inflate/deflate, pinch, crease and controlled remeshing; falls back to brush-style sculpt only when gestural editing is actually beneficial.
when_to_use: Use after primary forms are correct and organic volume/surface refinement is needed, or when a local form can be improved more reliably by sculpt-style operations than by ordinary mesh editing.
---

# Blender Organic Sculpting Skill

## 0. Philosophy

For an AI agent, sculpting should be controlled spatial editing, not random virtual brush waving.

Preferred abstraction:

```text
operation
+ target object
+ local/world center
+ radius
+ falloff
+ strength
+ symmetry
+ protected regions
```

Use deterministic regional operations whenever they can express the desired change.

## 1. Preconditions

Do not sculpt until:
- primary proportions pass;
- the target object is correct;
- reference constraints are known;
- a checkpoint/rollback path exists;
- the agent knows whether topology may be destroyed;
- symmetry intent is known.

Do not use Dyntopo or voxel remesh on final rigged topology unless the explicit task allows destructive topology replacement.

## 2. Sculpt modes

### Mode A — Deterministic regional sculpt (preferred)

Operations:
```text
DEFORM / GRAB-LIKE
SMOOTH
INFLATE
DEFLATE
PINCH
CREASE
FLATTEN / PLANARIZE
RELAX
```

Parameters:
```text
object_name
center
radius
strength
falloff
axis/delta
symmetry
mask/protected set
iterations
```

### Mode B — Blender Sculpt Mode

Use when:
- gestural surface flow matters;
- the form is too organic for simple region operators;
- a human-like brush stroke is specifically beneficial.

Still constrain:
- brush;
- radius;
- strength;
- stroke path;
- symmetry;
- number of passes.

Avoid long uncontrolled stroke sequences.

### Mode C — Volume reset

Use:
- voxel remesh;
- Dyntopo;
- remesh + smooth;
only during an early/mid sculpt stage when topology is explicitly disposable.

## 3. Coordinate contract

Every deterministic sculpt action must state coordinate space:
- object-local preferred for reusable character operations;
- world-space only when scene positioning is part of the task.

For bilateral characters, define symmetry plane explicitly.

Before a regional operation:
1. get object transform;
2. convert target landmark to chosen space;
3. calculate radius relative to local feature scale;
4. execute;
5. validate fixed views.

## 4. Landmark-driven regions

Do not choose centers by vague visual guess when rig/reference landmarks exist.

Useful landmarks:
- brow;
- cheek;
- jaw angle;
- chin;
- shoulder cap;
- deltoid insertion;
- elbow;
- forearm belly;
- wrist;
- chest;
- ribcage edge;
- pelvis rim;
- glute mass;
- knee cap;
- calf;
- ankle.

Region radius should be derived from nearby form scale where possible.

Example:
```text
center = shoulder_landmark + arm_axis * 0.35 * upper_arm_length
radius = 0.45 * upper_arm_radius
operation = inflate
amount = small
```

Task-derived values replace generic numbers.

## 5. Deterministic falloff

Common falloffs:

### Linear
Good for mechanical/simple transitions.

### Smoothstep
Default for organic form:
```text
t = clamp(distance / radius, 0, 1)
weight = 1 - (3*t^2 - 2*t^3)
```

### Gaussian-like
Useful for broad soft volume.

### Sharp
Use sparingly for creases/ridges.

Avoid discontinuous changes that create rings at the influence boundary.

## 6. DEFORM / grab-like operation

Purpose:
- move a bounded region while preserving surrounding shape.

Algorithm:
1. collect vertices within radius;
2. compute falloff weight;
3. apply delta * weight * strength;
4. optionally project movement along selected axis/normal;
5. mirror if required;
6. smooth only the boundary if necessary.

Use for:
- jaw projection;
- shoulder reposition;
- nose/chin stylization;
- cloth bulges;
- limb silhouette correction.

## 7. INFLATE / DEFLATE

Move vertices along:
- vertex normals;
- averaged local normals;
- radial vector from center;
depending on desired form.

Use for:
- muscle/soft volume;
- cheek;
- knuckle mass;
- boot/glove volume;
- controlled concavity.

Do not inflate an entire region uniformly if it destroys silhouette taper.

## 8. SMOOTH

Prefer adjacency-based Laplacian-like smoothing with boundary protection.

Rules:
- low strength, multiple bounded iterations;
- preserve silhouette vertices when silhouette is already locked;
- avoid shrinking thin forms;
- do not smooth away intentional crease edges.

If smoothing visibly changes the approved silhouette, reduce region or use tangential relaxation.

## 9. PINCH

Pull vertices toward:
- center;
- an axis;
- a curve/crease path.

Use for:
- cloth gathers;
- controlled taper;
- eyelid/eye socket definition;
- belt/strap transitions.

Pinch should not create self-intersection.

## 10. CREASE

Prefer a crease operation defined by:
- center/path;
- radius;
- depth;
- pinch;
- falloff.

Use for:
- cloth folds;
- mouth/brow groove in stylized characters;
- separation line between forms.

Creases are secondary/tertiary. Do not use them to fake missing primary volume.

## 11. FLATTEN / PLANARIZE

Fit a local plane from selected vertices or use a reference plane.

Useful for:
- stylized boot sole;
- armor plate;
- contact surfaces;
- deliberately flattened face planes.

Avoid accidental loss of curvature outside the bounded region.

## 12. Symmetry

Use symmetry while the design is symmetric.

Rules:
- verify object origin and symmetry plane;
- avoid double-applying to centerline vertices;
- stop symmetry before intentional asymmetry work;
- never "repair" intentional asymmetry automatically.

## 13. Dyntopo

Dyntopo is a topology-destructive sculpt representation.

Use only when:
- topology is disposable;
- major/secondary organic form still changes;
- retopology is planned afterward.

Before enabling:
- save checkpoint;
- record scale;
- record required surface detail;
- confirm no production skin weights must survive.

After:
- inspect holes/self-intersections;
- normalize detail only where needed;
- disable before handoff.

## 14. Voxel remesh

Use to unify/redistribute sculpt geometry, not as a universal cleanup button.

Choose voxel size based on smallest form that must survive.

If remesh erases:
- eyelids;
- fingers;
- thin cloth;
- accessory gaps;
then resolution is too coarse or remesh should not be used.

## 15. Sculpt pass order

```text
MACRO
→ MEDIUM
→ TRANSITIONS
→ FOLDS/CREASES
→ SURFACE CLEANUP
```

### Macro
- silhouette;
- head shape;
- torso mass;
- limb taper.

### Medium
- joint transitions;
- muscle/soft volume;
- cloth mass;
- cheeks/jaw.

### Fine
- folds;
- small creases;
- subtle surface character.

Never begin Fine while Macro fails.

## 16. Protected regions

When the task targets one area, define protected regions.

Example:
```text
TARGET: right forearm
PROTECT:
- shoulder location
- elbow landmark
- wrist landmark
- hand size
- torso silhouette
- left side
```

This prevents sculpt drift.

## 17. Anti-degradation loop

For every nontrivial pass:

```text
BEFORE evidence
→ bounded sculpt
→ AFTER evidence
→ compare
→ KEEP / CORRECT / REVERT
```

Mandatory questions:
```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

For sculpt-stage disposable topology, "topology became worse" means unusable surface structure, holes, foldovers, or excessive density rather than final quad flow.

## 18. Stop conditions

Stop sculpting when:
- requested form is achieved;
- repeated sculpt passes add noise rather than structure;
- remaining problems are topology problems;
- remaining problems are rig/weight problems;
- reference mismatch requires landmark correction rather than more surface manipulation.

## 19. Handoff to retopology

Provide:
```text
HIGH-RES SOURCE:
LOCKED SILHOUETTE:
JOINT LANDMARKS:
MUST-PRESERVE CREASES:
THIN PARTS:
EXPECTED DEFORMATION:
SYMMETRY STATUS:
```

## 20. Implementation note for Blender MCP

If dedicated sculpt-region tools are unavailable but bounded Python execution exists, implement equivalent operations with:
- `bpy`;
- `bmesh`;
- mesh vertex adjacency;
- object/world coordinate transforms.

Prefer small reusable helper functions over one large script.

The algorithmic contract matters more than the UI tool used.
