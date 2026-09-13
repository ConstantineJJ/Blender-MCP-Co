---
name: Blender Character Rigging Animation Godot
version: 2.0
description: Universal Blender character rigging, skinning, animation, secondary-motion preparation, and Godot-oriented GLB export skill, with a contained Stickmans Duel production profile.
when_to_use: Use for armatures, bones, skinning, weight repair, Actions, loops, attack animation, cloth-bone chains, secondary motion preparation, GLB export, and Godot import compatibility.
---

# Blender Character Rigging, Animation & Godot Skill

## 0. Scope

This skill owns:
- armature creation/editing;
- bone hierarchy;
- skinning;
- weight validation;
- animation Actions;
- secondary-motion bone chains;
- animation cleanup;
- GLB export;
- Godot-oriented validation.

It does not own broad character remodeling unless geometry blocks rig/animation and the user authorizes it.

## 1. Scope lock for rig tasks

For prompts such as:
- "add these four clips";
- "fix one bone";
- "repair the left shoulder weights";
- "export GLB";

do not:
- rebuild the rig;
- rename unrelated bones;
- change baseline Actions;
- remodel the character;
- alter materials;
unless required and explicitly in scope.

## 2. Preflight

Inspect:
- Blender version;
- target armature;
- mesh objects;
- current modifiers;
- current bone names;
- hierarchy;
- rest pose;
- vertex groups;
- existing Actions;
- target engine/export format.

Record `MUST PRESERVE`.

## 3. Armature design principles

A good production skeleton has:
- stable semantic names;
- unambiguous parent-child hierarchy;
- joint positions aligned with intended deformation;
- consistent left/right naming;
- intentional bone roll;
- predictable local axes;
- only necessary deform bones.

Control bones may be added for Blender authoring if export policy excludes or maps them appropriately.

## 4. Rest pose

Choose a rest pose that supports:
- clean weights;
- expected animation range;
- reference proportions;
- engine import.

Do not casually change rest pose after production animation exists.

Changing rest pose can invalidate:
- weights;
- Actions;
- constraints;
- export assumptions.

## 5. Bone placement

Place joint pivots from geometry/landmarks, not arbitrary visual centers.

Check:
- shoulder rotation center;
- elbow center;
- wrist;
- hip;
- knee;
- ankle;
- neck/head;
- pelvis/spine chain.

Side and front views must agree.

## 6. Bone orientation and roll

Consistency is more important than a favorite universal axis convention.

Within symmetric limb pairs:
- local axes should behave predictably;
- mirrored motion should not require random sign changes;
- roll should not flip between segments.

Before animation, test simple single-axis rotations.

## 7. Skinning workflow

Recommended:
1. parent/bind;
2. generate baseline weights if useful;
3. normalize;
4. remove stray weights;
5. test extreme poses;
6. correct critical joints;
7. retest.

Do not judge weights only in neutral pose.

## 8. Weight ownership diagnosis

When deformation is bad, identify whether cause is:
- weights;
- topology;
- bone placement;
- rest pose;
- constraint;
- animation.

Avoid endlessly repainting weights when the joint pivot is wrong.

## 9. Animation Action discipline

Each production clip should:
- have exact stable name;
- have clear frame range;
- contain only intended channels;
- avoid duplicate `.001` names;
- use consistent FPS assumption;
- define loop behavior;
- define root-motion policy.

Do not overwrite baseline clips when adding new ones.

## 10. Animation construction

For an action, establish:
```text
intent
→ key extremes
→ timing
→ arcs/spacing
→ overlap/follow-through
→ cleanup
→ loop/contact validation
```

For attacks:
```text
anticipation / windup
→ acceleration
→ contact/extreme
→ recoil/recovery
```

The visual contact frame should be readable even if game hit timing is controlled separately.

## 11. Punch rules

A convincing punch usually needs:
- shoulder contribution;
- torso contribution where style allows;
- clear elbow extension;
- fist travel;
- readable anticipation;
- recovery.

Avoid "mini-push" animation where the fist travels only a short distance.

If the task says legs stay idle during a punch, preserve lower-body baseline rather than adding unnecessary stepping.

## 12. Kick rules

Check:
- support leg;
- pelvis shift;
- knee chamber;
- extension;
- torso counterbalance;
- recovery.

For high kicks, verify head-height reach from the actual gameplay side view.

For sweeps, preserve low readable trajectory and support balance.

## 13. Locomotion

For in-place clips:
- no net root translation;
- foot timing readable;
- forward/backward character facing policy respected;
- first/last frames loop cleanly.

Backward locomotion should not accidentally turn the character if the project requires face-forward backpedaling.

## 14. Jump set

Typical division:
```text
jump_start
air_up
fall
land
```

or project-specific equivalent.

Keep root-motion policy explicit.

Landing must have a readable compression/recovery pose, not a nearly invisible transition.

## 15. Idle

Idle should:
- preserve recognizable combat stance;
- loop cleanly;
- have controlled torso/head/hand motion;
- avoid noisy constant movement;
- preserve planted-foot read when required.

Secondary hand circles/sway should support, not obscure, the stance.

## 16. Secondary motion

### Important GLB/Godot rule

A live Blender Cloth simulation does not automatically become live cloth simulation in Godot via ordinary GLB export.

Choose one:

1. **Bone-driven real-time secondary motion**
   - add cloth/accessory bone chain;
   - skin geometry to chain;
   - export bones;
   - drive in Godot with an appropriate spring/secondary-motion system.

2. **Baked animation**
   - bake simulated movement into bone/object animation when fixed playback is acceptable.

3. **Engine-native cloth/physics**
   - author geometry/rig for engine simulation.

For Stickmans Duel, cloth-like bandana/belt tails should prefer cloth-bone chains and Godot secondary motion when live motion is desired.

## 17. Cloth-bone chains

Good chain:
- anchored parent;
- 2–5+ segments depending on strip length;
- consistent axis/roll;
- progressive weights;
- no abrupt 100% weight boundary unless stylized;
- enough mesh segments to bend.

Test:
- gravity-like bend;
- turn lag;
- sudden attack;
- stop/recovery;
- no explosive flipping.

## 18. Constraints / IK

Use IK for authoring when helpful, but export policy must be explicit.

Before GLB:
- bake motion if target does not reproduce Blender constraints;
- verify final deform bones contain expected transforms;
- do not assume every Blender constraint is evaluated identically downstream.

## 19. Root motion

Root motion is a project decision.

Modes:
- full root motion;
- horizontal root motion;
- vertical root motion;
- in-place/no root motion.

Do not introduce root motion into an in-place project because it makes an animation look stronger.

## 20. Animation cleanup

Inspect:
- F-curves;
- accidental keyframes;
- Euler flips;
- scale keys;
- pelvis drift;
- limb pops;
- first/last loop mismatch;
- overshoot through body;
- foot sliding;
- constraint discontinuities.

Use quaternion/euler modes intentionally.

## 21. GLB export

Before export:
- select intended armature/meshes;
- verify transforms;
- verify Action names;
- verify NLA/Action export behavior;
- exclude refs/helpers;
- verify materials;
- verify modifiers;
- verify shape keys;
- verify animation range.

After export, fresh-import when possible.

## 22. Godot import validation

Verify:
- orientation;
- scale;
- mesh count;
- skeleton;
- bone names;
- animation list;
- animation lengths;
- materials;
- no forbidden root motion;
- no missing accessory bones.

If an animation appears wrong in Godot, determine whether defect originates in:
- Blender Action;
- GLB exporter;
- Godot importer;
- runtime animation blending.

## 23. Anti-degradation for rig/animation

After changes ask:

```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

Also:
```text
Did bone names/hierarchy change?
Did baseline Actions change?
Did weights regress?
Did export behavior change?
```

Unexpected YES → CORRECT or REVERT.

# 24. Project profile — Stickmans Duel

Apply this entire section only when the active task explicitly concerns Stickmans Duel.

## 24.1 Production rig contract

Fixed 18-bone names:

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

Rules:
- do not rename these bones;
- do not rebuild the rig when only animation is requested;
- preserve hierarchy unless the user explicitly orders a rig change;
- preserve compatibility across fighters using the standard rig.

## 24.2 Export

Production fighter format:
```text
GLB
```

Target project asset location is project-controlled; when the known Stickmans Duel path is active, export fighter GLBs under the project's `3D/Assets` convention.

Do not export extra helper rigs/reference objects.

## 24.3 Root motion

Stickmans Duel production locomotion and combat clips are **in-place / no root motion** unless the user explicitly changes the project rule.

Do not animate `Root` translation to fake movement.

## 24.4 Established baseline clip family

Current baseline family from the production character workflow includes:

```text
idle
walk_run
walk_backward
jump
jump_start
air_up
fall
land
attack_mid_hand
hit_mid
```

Treat existing baseline Actions as protected when the user asks only to add new clips.

## 24.5 Additional attack Actions

Established requested additions:

```text
attack_high_hand
attack_mid_leg
attack_high_leg
attack_low_sweep
```

When transferring/creating these on another identical fighter rig:
- do not rename bones;
- do not alter baseline Actions;
- do not rebuild rig;
- keep no-root-motion;
- use the same rig semantics.

## 24.6 Punch quality

Known project requirement:
- punch must read as a real extension, not a short mini-push;
- clear fist travel;
- full enough elbow extension;
- wider shoulder/torso contribution as appropriate;
- legs remain in idle stance when specifically requested.

## 24.7 Backward walk

`walk_backward`:
- in-place;
- fighter keeps facing opponent/front as required;
- do not simply rotate the whole character backward.

## 24.8 Idle reference

Established idle design:
- left leg forward;
- right leg back and bent;
- left arm forward/low;
- right arm back and bent;
- subtle torso sway;
- small circular hand motion.

Do not replace this with a generic symmetrical idle without user request.

## 24.9 Landing

Landing pose must be visually readable:
- clear knee/pelvis compression;
- short recovery;
- no root-motion displacement.

## 24.10 Accessories

Bandana/belt cloth:
- Blender Cloth may be used for authoring experiments;
- live Godot behavior should use exported cloth-bone chains + Godot secondary motion when appropriate;
- do not claim live Blender Cloth transfers through GLB.

## 24.11 Animation-only surgical task template

When user says:
> add X Actions to existing identical 18-bone rig

Do exactly:

```text
1. inspect rig and existing Actions
2. verify all 18 required bones
3. verify protected baseline Action names
4. create only requested Actions
5. key only necessary bones
6. validate extreme/contact poses
7. verify no root motion
8. verify baseline Actions unchanged
9. save/export only if requested
10. stop
```

Do not remodel, rename, or "improve" unrelated character parts.

## 25. Completion criteria

Rigging/animation task passes when:
- requested rig/Action change exists;
- protected rig data remains intact;
- deformation is acceptable at relevant poses;
- animation intent reads at game camera;
- root-motion policy is obeyed;
- GLB/Godot validation passes when export is part of scope.
