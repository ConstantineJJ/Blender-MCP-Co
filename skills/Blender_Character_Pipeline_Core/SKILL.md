---
name: Blender Character Pipeline Core
version: 1.0
description: Universal orchestration skill for character creation in Blender. Routes tasks to the smallest necessary specialist skills, enforces scope lock, stage gates, handoff contracts, fixed-view validation, and anti-degradation.
when_to_use: Use for any multi-stage character task, a new character, major revision, reference reconstruction, sculpt-retopo-rig pipeline, or whenever several Blender character skills could conflict.
---

# Blender Character Pipeline Core

## 0. Purpose

This is the routing and governance layer for the Blender Character Agent Pipeline.

It does not replace specialist skills. It decides:
- which skills are needed;
- in what order they run;
- which evidence each stage must produce;
- which invariants must be preserved;
- when work is allowed to advance;
- when a change must be reverted.

The goal is predictable character work from short user prompts.

## 1. Authority and conflict order

When instructions conflict, use this order:

1. The user's current explicit task.
2. Explicit project-specific requirements.
3. Locked reference contract / approved visual decisions.
4. This Pipeline Core.
5. Specialist skill instructions.
6. Generic Blender defaults and conventions.

Never silently replace a project rule with a generic best practice.

If the user asks for a narrow change, the narrow task wins. Do not use the existence of this pipeline as permission to remodel, rerig, retexture, rename, or "improve" unrelated work.

## 2. Scope Lock — mandatory

Before editing, write an internal scope contract:

```text
TASK:
TARGET OBJECTS:
ALLOWED CHANGES:
MUST PRESERVE:
EXPLICIT EXCLUSIONS:
DELIVERABLE:
VALIDATION:
```

Rules:
- Implement only the requested task and the minimum necessary dependencies.
- Useful ideas outside scope go into Suggestions only. Do not execute them.
- Do not opportunistically "clean up" unrelated objects, modifiers, actions, names, materials, rigs, or scene settings.
- Do not rebuild a working subsystem just because another method is preferred.
- Do not spend time re-proving already established project facts unless new evidence contradicts them.
- If the requested task is surgical, use Surgical Mode.

### Surgical Mode

Use for prompts such as:
- "make the head 8% larger";
- "fix the right forearm";
- "add four Actions";
- "change the belt color";
- "repair shoulder weights".

Procedure:
1. Inspect only the relevant dependencies.
2. Record invariants.
3. Make the smallest change.
4. Run focused validation.
5. Stop.
6. Mention unrelated issues only as Suggestions.

## 3. Skill routing

Load the smallest set that covers the task.

### New reference-based character

```text
Pipeline Core
→ Reference Reconstruction
→ Character Modeling
→ Character QA
→ Organic Sculpting if needed
→ Retopology & Deformation if needed
→ Character QA
→ Rigging / Animation / Godot if required
→ Final QA
```

### Existing model refinement

```text
Pipeline Core
→ Character QA
→ Iterative Refinement
→ specialist responsible for the error
→ same Character QA
```

### Sculpt-only task

```text
Pipeline Core
→ Organic Sculpting
→ Character QA
```

### Retopology / deformation repair

```text
Pipeline Core
→ Retopology & Deformation
→ Character QA
```

### Rig / animation / export only

```text
Pipeline Core
→ Rigging / Animation / Godot
→ Character QA
```

### Reference mismatch

If the user says:
- "does not look like the reference";
- "wrong silhouette";
- "wrong proportions";
- "front is right but side is wrong";

stop generic modeling retries and load:
1. Reference Reconstruction,
2. Character QA,
3. Iterative Refinement,
then the specialist required by the diagnosed error.

## 4. Production stages

Default order:

```text
REFERENCE CONTRACT
↓
BLOCKOUT
↓
PRIMARY FORMS
↓
PRIMARY QA GATE
↓
SECONDARY FORMS
↓
SCULPT / FORM REFINEMENT
↓
TOPOLOGY
↓
DEFORMATION QA GATE
↓
DETAIL / MATERIALS
↓
MULTIVIEW QA
↓
ITERATIVE REFINEMENT
↓
RIG
↓
ANIMATION
↓
EXPORT
↓
FRESH-IMPORT QA
```

Stages may be skipped only when they are irrelevant to the user task.

## 5. Stage gates

A gate is an internal quality condition, not a request for user approval unless the task is genuinely ambiguous.

Do not stop after every stage to ask permission.

### Reference Gate

Must know:
- intended subject;
- reference role: inspiration, shape-locked, or orthographic contract;
- primary silhouette view;
- known dimensions or proportion unit;
- structural part list;
- explicit user constraints.

### Primary Form Gate

Do not add fine details while any critical primary-form issue remains.

Check:
- total height / width / depth;
- head-to-body ratio;
- shoulder / hip width;
- torso length;
- limb segment ratios;
- hand / foot scale;
- major silhouette;
- front/side/back agreement.

### Sculpt Gate

Before sculpting:
- primary proportions pass;
- target objects are identified;
- symmetry intent is known;
- topology destruction risk is acceptable;
- a rollback/checkpoint exists.

### Retopology Gate

Before final topology:
- macro form is approved by the evidence;
- no large sculpt correction is still expected;
- deformation regions are known.

### Rig Gate

Before rigging:
- transforms and object scale are intentional;
- topology is suitable for required deformation;
- joint landmarks are stable;
- object naming is stable enough for binding.

### Export Gate

Before export:
- expected objects only;
- expected Actions only;
- transforms verified;
- normals verified;
- no accidental reference objects;
- target format rules verified;
- fresh-import test when practical.

## 6. Handoff contract between skills

Each specialist should hand off a compact status:

```text
STAGE:
CHANGED:
PRESERVED:
EVIDENCE:
OPEN DEFECTS:
HARD BLOCKERS:
NEXT SKILL:
```

Never rely on vague statements such as "looks good".

Use stable object, material, armature, bone, and Action names in reports.

## 7. Deterministic execution principles

Prefer deterministic operations whenever practical:
- named objects instead of current selection;
- explicit coordinates and dimensions;
- explicit modifier settings;
- explicit bone names;
- fixed camera transforms for comparison;
- stable reference landmarks;
- scripted mesh operations for repeatable sculpt changes.

When Blender MCP provides general Python execution, generated Python may implement bounded helper operations. Keep each operation:
- scoped;
- reversible or checkpointed;
- name-addressed;
- independently verifiable.

Do not generate one giant monolithic script for a long character task.

## 8. Anti-degradation contract — mandatory for revisions

Before a meaningful revision, record:
- approved silhouette;
- approved proportions;
- object hierarchy;
- important names;
- topology state;
- rig state;
- animation state;
- material assignments;
- export expectations.

After the change ask, explicitly:

```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

Also check task-specific invariants.

Decision:

```text
requested improvement + no material regression → KEEP
requested improvement + local regression → CORRECT
no meaningful improvement → REVERT
net quality worse → REVERT
```

Never keep a change merely because substantial work was spent on it.

## 9. Fixed-view evidence

For character geometry work, default evidence views are:

```text
FRONT
BACK
LEFT
RIGHT
3/4 FRONT
3/4 BACK
```

Optional:
```text
TOP
BOTTOM
POSE TESTS
WIREFRAME
WEIGHT VISUALIZATION
```

Use the same:
- orthographic/perspective mode;
- camera location;
- focal length;
- framing;
- lighting;
- render resolution;

when comparing before vs after.

Changing the camera between comparisons invalidates visual evidence.

## 10. Error priority

Repair in this order unless the user's task specifies otherwise:

1. Wrong identity / reference interpretation.
2. Wrong overall silhouette.
3. Wrong global proportions.
4. Wrong structural relationships.
5. Bad deformation zones.
6. Topology defects that block deformation/export.
7. Secondary forms.
8. Materials / surface response.
9. Tertiary detail.

Do not polish a wrong silhouette.

## 11. Failure escalation

If two consecutive attempts at the same correction fail:
- stop repeating the same method;
- identify why it failed;
- switch representation or tool family;
- capture evidence;
- use Iterative Refinement.

Examples:
- vertex nudging fails → use landmark-driven proportional edit;
- brush sculpt drifts → use deterministic region deformation;
- repeated retopo repair fails → rebuild only the affected patch;
- side view keeps breaking front → lock front coordinates and constrain side to depth only.

## 12. Destructive operations

Treat as high-risk:
- voxel remesh;
- Dyntopo;
- applying destructive modifiers;
- deleting armatures or Actions;
- rebinding meshes;
- joining/splitting production meshes;
- baking over only copies;
- changing rest pose;
- renaming bones used downstream.

Before such operations:
1. verify they are in scope;
2. record invariants;
3. create a recoverable checkpoint according to the project's backup policy;
4. perform the smallest destructive operation possible;
5. run immediate QA.

## 13. Stop conditions

Stop when:
- the explicit task is complete;
- acceptance checks pass;
- no required gate is failing;
- remaining observations are outside scope.

Do not turn a narrow request into a full character production pass.

## 14. Final report format

```text
Completed:
- ...

Validated:
- ...

Preserved:
- ...

Remaining:
- only requested/open issues

Suggestions:
- optional, not executed
```

Keep the user-visible report compact unless they ask for a detailed production report.
