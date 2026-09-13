---
name: Blender Character Modeling
version: 2.0
description: Universal production skill for building stylized or semi-realistic Blender characters from blockout through primary and secondary forms while preserving reference contracts, deformation needs, and downstream rig/export requirements.
when_to_use: Use for creating or substantially editing character geometry. Pair with Reference Reconstruction for reference-locked work, Organic Sculpting for freeform organic refinement, and Retopology & Deformation for final animation-ready topology.
---

# Blender Character Modeling Skill

## 0. Scope

This skill owns:
- blockout;
- primary forms;
- secondary forms;
- modular character parts;
- clothing/accessory geometry;
- production-oriented modifier use;
- geometry preparation for sculpt/retopo/rig.

It does not own:
- freeform sculpt passes;
- final retopology strategy;
- rigging/weighting;
- animation;
- Godot-specific export validation.

## 1. First rule: build the right large forms before details

Production order:

```text
blockout
→ primary silhouette
→ major volumes
→ joint/deformation allowances
→ secondary forms
→ clothing/accessories
→ controlled surface refinement
→ handoff
```

Do not model wrinkles, seams, fingernails, micro-bevels, or decorative details while major proportions are wrong.

## 2. Read the task contract

Before editing, determine:
- new model or revision;
- reference mode;
- target style;
- target engine/render use;
- expected deformation;
- poly/detail target;
- symmetry;
- modularity;
- export constraints;
- explicit exclusions.

For a narrow change, use the Pipeline Core Surgical Mode.

## 3. Blockout strategy

Use simple geometry chosen for editability.

Typical:
- UV sphere / ico sphere: head, joint caps;
- cylinder/capsule-like mesh: limbs;
- low-resolution custom mesh: torso/pelvis;
- curves with bevel: ropes/tubes before conversion;
- cubes with bevel/subdivision: stylized boots/gloves/armor;
- mirrored half-mesh: symmetric body shells.

Blockout objects should already carry semantic names.

Avoid leaving production objects named `Cube.013`, `Sphere.006`, etc.

## 4. Primary forms

Primary forms determine character identity.

Check:
- head size and shape;
- torso taper;
- shoulder width;
- pelvis width;
- limb length;
- hand/foot mass;
- stance-neutral limb clearance;
- major costume silhouette.

For stylized characters, intentional exaggeration beats generic "correct anatomy".

If a reference exists, obey its proportion table.

## 5. Joint-aware modeling

Even before rigging, reserve deformation structure.

### Shoulder
Needs enough volume/clearance for:
- arm forward;
- arm raised;
- arm across chest.

Avoid a spherical shoulder merely intersecting the torso unless the project deliberately uses separate rigid pieces.

### Elbow
Provide a bend region, not a single razor-thin edge ring.

### Wrist
Keep enough length between hand mass and forearm taper.

### Hip
Avoid merging thigh into pelvis with an uncontrolled concave pinch.

### Knee
Preserve front/back mass and room for compression.

### Ankle
Avoid an abrupt zero-volume hinge unless intentionally mechanical.

## 6. Separate pieces vs continuous mesh

Use separate pieces when they are genuinely separate:
- armor;
- bandages;
- belt;
- headband;
- accessories;
- rigid boots/gloves;
- cloth strips with separate simulation/bone behavior.

Use a continuous mesh when a continuous deformation surface is needed.

Do not join geometry just to reduce object count.

## 7. Modifier policy

Modifiers are tools, not trophies.

Common safe order depends on intent, but often:
```text
Mirror
→ structural modifiers
→ Bevel / support
→ Subdivision
→ corrective modifiers
```

Do not assume one stack order fits every character.

Rules:
- keep Mirror unapplied while symmetry is still useful;
- apply scale before modifiers that depend on real distances when appropriate;
- inspect Boolean topology before subdivision;
- do not apply Subdivision merely to increase vertex count;
- avoid unapplied modifier surprises at export.

Record which modifiers must remain editable and which must be applied before handoff/export.

## 8. Surface continuity

Inspect:
- visible shading dents;
- non-planar accidental flats;
- pinching near poles;
- lumpy cylinder transitions;
- inconsistent radius along limbs;
- accidental asymmetry;
- intersections exposed by movement.

Smooth shading does not repair bad geometry.

## 9. Stylized hands

Choose hand complexity to match camera and gameplay.

Possible levels:
1. mitten/sphere;
2. glove with thumb;
3. simplified fingers;
4. articulated fingers.

Do not spend topology budget on fingers that never read in the target camera.

If interaction/grabbing is important, hand silhouette and thumb opposition may justify extra structure.

## 10. Face and eye geometry

For simple stylized faces:
- prioritize readable eye silhouette;
- avoid floating eyes unless intended;
- keep depth/recess consistent;
- test from game camera distance;
- maintain symmetry unless expression/design requires asymmetry.

For eye pieces mounted on a curved head:
- conform to surface or intentionally recess;
- avoid z-fighting;
- verify side view.

## 11. Clothing and cloth-like accessories

Classify each as:
- rigid geometry;
- skinned cloth;
- cloth-bone chain;
- baked cloth;
- real-time engine simulation.

Do not assume Blender Cloth simulation transfers as live cloth behavior through GLB.

Geometry for cloth-bone chains should:
- have enough longitudinal segments;
- avoid unnecessary radial density;
- place pivots/weights to support smooth bending.

## 12. Detail budget

Use three levels:

### Primary
Read from far away.
- silhouette;
- body proportions;
- big clothing masses.

### Secondary
Read at normal gameplay / medium render distance.
- gloves;
- boots;
- folds;
- belts;
- headbands;
- muscle/volume transitions.

### Tertiary
Read in close-up.
- tiny wrinkles;
- stitching;
- pores;
- micro-bevels.

Never allow tertiary work to consume time needed to fix primary errors.

## 13. Naming

Recommended universal prefixes:
```text
GEO_
ARM_
MAT_
REF_
COL_
```

Project conventions may override.

Use semantic names:
```text
GEO_Body
GEO_Head
GEO_Belt
GEO_HeadbandTail_L
```

Avoid names that encode temporary topology state.

## 14. Transform discipline

Before rig/animation handoff:
- object scale should be intentional;
- rotations should be intentional;
- origin placement should support downstream use;
- mirrored objects should not contain accidental negative-scale traps;
- scene unit scale should be known.

Do not casually apply armature transforms after binding.

## 15. Modeling QA checkpoints

After blockout:
- front/side/back silhouette.

After primary forms:
- proportions and intersections.

After secondary forms:
- 3/4 views and deformation clearance.

Before handoff:
- normals;
- non-manifold checks;
- duplicate vertices;
- internal accidental faces;
- transform sanity;
- modifier sanity;
- object naming.

## 16. Handoff to Organic Sculpting

Send:
```text
LOCKED PRIMARY FORMS:
SCULPT TARGET OBJECTS:
SYMMETRY:
AREAS TO REFINE:
AREAS TO PRESERVE:
TOPOLOGY MAY BE DESTROYED: yes/no
```

Do not send an underdefined "make it better" sculpt task.

## 17. Handoff to Retopology

Send:
```text
FINAL MACRO FORM:
EXPECTED DEFORMATION:
JOINT LOCATIONS:
CLOTHING LAYERS:
RIG TARGET:
POLY TARGET:
EXPORT TARGET:
```

## 18. Anti-degradation during modeling revisions

After a revision ask:
```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

If a head fix improves front view but ruins profile, the fix is incomplete.

## 19. Project profile — Stickmans Duel

Apply this section only when the task explicitly concerns Stickmans Duel.

### Current character pipeline assumptions
- True 3D character used in 2.5D gameplay.
- Movement is locked to a gameplay plane in Godot.
- Orthographic side-camera readability matters strongly.
- GLB is the production interchange format for fighters.
- Character silhouettes must remain readable at game-camera distance.

### Standard skeleton compatibility
Model around this fixed 18-bone naming contract when the task requires the production fighter rig:

```text
Root
Pelvis
Spine
Chest
Neck
Head
L UpperArm
L Forearm
L Hand
R UpperArm
R Forearm
R Hand
L Thigh
L Shin
L Foot
R Thigh
R Shin
R Foot
```

Do not rename these bones from the modeling stage merely for personal preference.

### Existing visual lessons
For the established stickman family, preserve when applicable:
- large readable head;
- recessed/semi-spherical eye treatment when specified;
- forearm may intentionally read longer than upper arm;
- clean pelvis→thigh→shin connections;
- readable boots/feet;
- accessories should not obscure eye read;
- simple bold forms are more important than dense surface detail.

### Cloth/accessories
Headband/belt tails should be modeled so they can use:
- cloth-bone chains;
- procedural/spring secondary motion in Godot;
rather than depending on live Blender Cloth transfer.

### Scope rule
If the user says "only add animations", "only fix the forearm", etc., do not remodel the rest of the fighter.

## 20. Completion criteria

Character Modeling is complete when:
- the intended character reads from required views;
- primary proportions pass;
- major intersections are resolved;
- geometry is suitable for the next stage;
- no out-of-scope redesign was introduced.
