---
name: Blender Character QA
version: 1.0
description: Evidence-based multiview, topology, deformation, rig, animation, and export validation for Blender characters.
when_to_use: Use at stage gates, before/after significant revisions, before export, after export reimport, or whenever an agent is about to claim a character/model/animation is finished.
---

# Blender Character QA Skill

## 0. Rule

No "looks good" without evidence.

A successful API call proves the command ran. It does not prove the character is correct.

QA combines:
- deterministic scene inspection;
- fixed-view visual evidence;
- topology checks;
- deformation tests;
- animation checks;
- export/fresh-import checks.

## 1. QA profiles

Select only relevant profiles.

### Geometry QA
For modeling/sculpt.

### Topology QA
For retopo.

### Deformation QA
For skinning/rig changes.

### Animation QA
For Actions/motion.

### Export QA
For GLB/FBX/etc.

### Full Character QA
Before production acceptance.

## 2. Fixed diagnostic views

Default character set:
```text
FRONT
BACK
LEFT
RIGHT
3/4 FRONT
3/4 BACK
```

Additional when relevant:
```text
TOP
WIREFRAME FRONT
WIREFRAME 3/4
POSE CONTACT SHEET
WEIGHT VIEW
SILHOUETTE MASK
```

Store cameras or deterministic camera transforms.

Before/after evidence must use the same camera and framing.

## 3. Neutral presentation

Diagnostic lighting should reveal form, not hide defects.

Avoid:
- extreme rim-only lighting;
- depth-of-field that blurs geometry;
- dramatic perspective when checking orthographic proportions;
- dark materials merging into dark background.

Hero renders are separate from QA views.

## 4. Geometry checks

Check:
```text
[ ] correct object count
[ ] semantic names
[ ] intended symmetry/asymmetry
[ ] expected transforms
[ ] no obvious accidental intersections
[ ] no duplicate visible shells
[ ] normals correct
[ ] no missing faces
[ ] no unintended internal geometry
[ ] reference objects excluded from export
```

## 5. Silhouette checks

At minimum front and side.

Inspect:
- head/body ratio;
- shoulder width;
- arm/leg taper;
- hand/foot size;
- clothing/accessory outline;
- negative spaces between limbs/body;
- unintended lumps.

Silhouette defects outrank micro surface polish.

## 6. Reference checks

When reference-locked:
- compare same projection;
- verify landmark positions;
- verify visible part count;
- compare bounding box and center;
- use overlays/masks if available;
- record largest mismatch.

Do not accept a model because a 3/4 beauty render resembles the concept while front/side are wrong.

## 7. Topology checks

Report:
- vertex/edge/face counts;
- non-manifold edges;
- loose geometry;
- duplicate vertices;
- degenerate faces;
- very thin accidental triangles;
- poles in critical deform zones;
- mirrored seam integrity.

"High poly" is not itself a defect. Unjustified density is.

## 8. Modifier checks

Verify:
- intended modifier order;
- disabled/render-only discrepancies;
- accidental unapplied destructive dependencies;
- export behavior;
- Mirror seam;
- Subdivision level;
- Boolean leftovers.

## 9. Transform checks

Verify:
- unit scale;
- object scale;
- object rotation;
- origin;
- armature transform;
- negative scales;
- parent hierarchy.

Do not auto-apply transforms to production rigs during QA. Diagnose first.

## 10. Deformation QA

Use required pose tests.

Record for each pose:
```text
POSE:
PASS/FAIL:
DEFECT REGION:
TYPE:
SEVERITY:
LIKELY OWNER:
```

Likely owner:
- topology;
- weights;
- bone placement;
- animation;
- cloth/accessory system.

## 11. Rig QA

Check:
- expected bone count;
- expected bone names;
- hierarchy;
- disconnected/unintended bones;
- deform flags;
- roll/orientation;
- constraints;
- IK/FK state if relevant;
- no accidental scale animation;
- no rest-pose drift.

Project-specific contracts override generic expectations.

## 12. Animation QA

For each required Action:
- name exists;
- frame range exists;
- no accidental duplicate suffix;
- intended bones keyed;
- unintended bones not keyed;
- first/last loop continuity when looping;
- no foot sliding beyond intended style;
- no visible mesh explosions/intersections;
- no forbidden root motion;
- no unexpected object transform tracks.

Contact sheet should include:
```text
start
anticipation
contact/extreme
recovery
end
```
as applicable.

## 13. Export QA

Before export:
- selected object set correct;
- no reference helpers;
- no debug cameras/lights unless needed;
- materials expected;
- animations expected;
- shape keys expected;
- transforms expected;
- export format settings correct.

After export, when practical:
1. import into a clean Blender scene or clean collection;
2. inspect object/armature/action counts;
3. render diagnostic views;
4. compare with authored scene.

Fresh import catches exporter-only failures.

## 14. Godot-oriented GLB QA

When target is Godot:
- GLB opens;
- armature imported;
- expected animations visible;
- no accidental root motion when prohibited;
- bone names preserved as required;
- mesh scale/orientation correct;
- material appearance acceptable;
- cloth behavior expectations are not confused with baked Blender simulation.

## 15. Severity

### BLOCKER
Cannot proceed/export.
Examples:
- missing required bones;
- broken silhouette;
- non-manifold hole in visible body;
- animation missing;
- wrong export scale.

### MAJOR
Visible quality/function defect.
Examples:
- shoulder collapse;
- wrong limb proportion;
- severe cloth intersection.

### MINOR
Does not block stage.
Examples:
- subtle shading unevenness;
- small hidden topology inefficiency.

### NOTE
Optional improvement outside current scope.

## 16. Anti-degradation report

After revision answer:

```text
Did requested feature improve? YES/NO
Did silhouette become worse? YES/NO
Did another view become worse? YES/NO
Did topology become worse? YES/NO
Did deformation zones become worse? YES/NO
```

If any regression is YES, classify severity and decide KEEP/CORRECT/REVERT.

## 17. QA output format

```text
QA PROFILE:
SCOPE:
RESULT: PASS / PASS WITH MINOR NOTES / FAIL

BLOCKERS:
- ...

MAJOR:
- ...

MINOR:
- ...

EVIDENCE:
- ...

REGRESSION CHECK:
- requested feature:
- silhouette:
- other views:
- topology:
- deformation:

NEXT OWNER:
- skill name
```

## 18. Stickmans Duel QA extension

Apply only for Stickmans Duel fighter assets.

Verify when relevant:
- fixed 18-bone production naming contract;
- no unrequested bone rename;
- required Actions only;
- no root motion;
- in-place locomotion clips;
- readable side-game silhouette;
- extreme punches/kicks do not expose catastrophic intersections;
- cloth tails are compatible with the intended Godot secondary-motion method;
- GLB import preserves clips/bones.

Use the exact project Action list from the Rigging/Animation/Godot skill or current project documentation when newer.
