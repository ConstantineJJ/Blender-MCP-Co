---
name: Blender Retopology and Deformation
version: 1.0
description: Animation-ready retopology, edge-flow, deformation preparation, skinning topology review, and pose-based deformation validation for Blender characters.
when_to_use: Use after sculpt/primary form is stable, before final rigging, or when an existing rig shows collapsing shoulders, elbows, hips, knees, wrists, ankles, or other topology-driven deformation failures.
---

# Blender Retopology & Deformation Skill

## 0. Goal

Create the simplest topology that:
- preserves the approved shape;
- deforms correctly for required motion;
- exports reliably;
- remains editable.

"All quads" is not the goal by itself. Good deformation and clean surface behavior are.

## 1. Preconditions

Before final retopology:
- primary form is stable;
- major sculpt corrections are done;
- joint centers are known;
- expected animation range is known;
- target engine/render budget is known.

Do not retopologize a moving target unless the user specifically requests an interim mesh.

## 2. Deformation-first planning

Mark deformation zones:
- shoulders;
- elbows;
- wrists;
- hips;
- knees;
- ankles;
- neck;
- jaw/face if animated;
- cloth folds that bend.

Mark rigid zones:
- armor plates;
- hard boots;
- hard gloves;
- accessories.

Topology density should follow deformation and silhouette needs, not arbitrary uniformity.

## 3. Edge-loop principles

### Elbow / knee
Use enough loops to:
- define joint volume;
- compress inside bend;
- stretch outside bend;
- avoid a single hinge edge.

### Shoulder
Shoulder deformation is 3D, not a simple cylinder bend.

Provide flow supporting:
- arm down;
- arm forward;
- arm raised;
- arm across body if required.

Avoid topology that sends all tension into the armpit.

### Hip
Support thigh forward/back/side motion without collapsing the pelvis.

### Wrist / ankle
Keep a stable transition so rotation does not create a candy-wrapper pinch.

### Neck
Support head rotation without pulling chest/shoulders unnaturally.

## 4. Quad policy

Prefer quads in:
- deforming regions;
- subdivision surfaces;
- visible smooth surfaces.

Triangles are acceptable when:
- they do not deform badly;
- they do not cause shading artifacts;
- they are intentional for game export.

N-gons are acceptable in rigid flat hidden regions if the downstream pipeline handles them, but avoid relying on them near deformation/subdivision.

## 5. Pole placement

Poles are not inherently defects.

Place them:
- away from high-bend centers;
- away from critical highlight flow;
- where topology direction legitimately changes.

Do not solve every topology problem by pushing a pole into the armpit or knee.

## 6. Retopo methods

Possible approaches:
- manual quad build;
- Shrinkwrap-assisted surface;
- BMesh scripted strips;
- symmetry + Mirror;
- selective patch rebuild;
- automatic retopo as a starting point only.

Auto-retopo must still pass deformation tests.

## 7. Density management

Density hierarchy:
1. silhouette;
2. joints;
3. face/hands when visible;
4. secondary folds;
5. broad flat surfaces.

Avoid dense hidden surfaces while a visible elbow has too few support loops.

## 8. Separate clothing

For deforming clothes:
- preserve adequate offset from body;
- avoid embedded vertices;
- allow motion without constant body intersection;
- use compatible but not necessarily identical edge flow.

If clothing is mostly rigid/stylized, simpler independent topology may be better.

## 9. Normals and shading

After retopo:
- recalculate intended normals;
- inspect hard/soft transitions;
- inspect mirrored seam;
- inspect subdivision;
- verify no inverted islands.

Custom normals may be used when the target pipeline supports them, but should not conceal broken geometry.

## 10. Skinning preparation

Before weights:
- transforms known;
- armature scale known;
- rest pose stable;
- mesh not accidentally parented twice;
- vertex groups named consistently if precreated.

Do not bake corrective topology assumptions around a temporary bone placement.

## 11. Weighting principles

When this skill is used to diagnose deformation, distinguish:
- topology error;
- weight error;
- bone-placement error;
- rest-pose error.

Do not rebuild topology for a problem caused only by weights.

Useful starting strategy:
- automatic weights as baseline when appropriate;
- normalize;
- remove stray low weights;
- manual correction at critical joints;
- preserve left/right symmetry until intentional asymmetry.

## 12. Deformation test pose set

Minimum humanoid test set:

```text
T/A neutral
arms forward
arms overhead
elbow ~90°
wrist flex/extend
deep hip flex
knee ~90°
deep knee bend
foot plantar/dorsi flex
torso twist
neck turn/tilt
```

Project-specific ranges override these.

## 13. Pose-based QA

For each critical pose inspect:
- volume preservation;
- joint collapse;
- candy-wrapper twist;
- armpit pinch;
- hip tearing;
- elbow/knee spikes;
- clothing intersections;
- normal/shading artifacts.

Test from:
- front;
- side;
- 3/4.

A deforming mesh that looks good only in rest pose is not finished.

## 14. Corrective strategy

Use this order:
1. verify bone joint location;
2. verify weights;
3. redistribute loops;
4. adjust local surface volume;
5. use corrective shape key only if needed.

Corrective shape keys should solve known extreme-pose issues, not compensate for generally bad topology.

## 15. Anti-degradation

Before topology repair record:
- silhouette;
- vertex count;
- seam locations;
- deformation behavior;
- material assignments;
- UV dependencies if any.

After:
```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

For topology tasks, also ask:
```text
Did UVs/material assignments break?
Did vertex groups break?
Did mirrored seam break?
```

## 16. Handoff to rigging

Provide:
```text
MESH OBJECTS:
REST POSE:
JOINT LANDMARKS:
KNOWN EXTREME POSES:
CLOTHING LAYERS:
CORRECTIVE SHAPES:
TOPOLOGY EXCEPTIONS:
```

## 17. Exit criteria

Retopology/deformation stage passes when:
- surface is clean at production views;
- critical joints survive required pose range;
- no topology defect blocks skinning/export;
- density is justified;
- silhouette remains faithful to approved form.
