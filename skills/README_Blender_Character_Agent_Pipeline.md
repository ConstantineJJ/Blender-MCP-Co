# Blender Character Agent Pipeline

**Version:** 1.0  
**Purpose:** universal Blender character-agent skill stack with a contained Stickmans Duel production profile.

The skill folders in this repository live directly under `skills/` and can be copied to:

```text
E:\MyCreations\Blender-MCP-Co\skills
```

## Included skills

```text
Blender_Character_Pipeline_Core/
└── SKILL.md

Blender_Reference_Reconstruction_SKILL/
└── SKILL.md

Blender_Character_Modeling_SKILL/
└── SKILL.md

Blender_Organic_Sculpting_SKILL/
└── SKILL.md

Blender_Retopology_Deformation_SKILL/
└── SKILL.md

Blender_Character_QA_SKILL/
└── SKILL.md

Blender_Iterative_Refinement_SKILL/
└── SKILL.md

Blender_Character_Rigging_Animation_Godot_SKILL/
└── SKILL.md
```

## Design goals

1. Short prompts should remain predictable.
2. Scope expansion is forbidden unless needed by the task.
3. Reference matching uses measurements and fixed views, not vague visual memory.
4. Primary forms must pass before detail work.
5. Sculpting is deterministic-first.
6. Destructive operations are checkpointed.
7. QA uses fixed-view evidence.
8. Revisions use anti-degradation.
9. The pipeline fixes one error class at a time.
10. Project-specific rules do not contaminate the universal base.

## Skill graph

```text
                         ┌────────────────────────────┐
                         │ Character Pipeline Core    │
                         └─────────────┬──────────────┘
                                       │
            ┌──────────────────────────┼──────────────────────────┐
            │                          │                          │
            v                          v                          v
  Reference Reconstruction    Character Modeling        Character QA
            │                          │                          │
            └──────────┬───────────────┘                          │
                       v                                          │
               Organic Sculpting                                  │
                       │                                          │
                       v                                          │
          Retopology & Deformation                                │
                       │                                          │
                       └───────────────> Character QA <────────────┘
                                          │
                                          v
                              Iterative Refinement
                                          │
                                          v
                         Rigging / Animation / Godot
                                          │
                                          v
                                 Final / Export QA
```

## Core scope rule

The user's current task is the highest authority.

Example:

```text
Add attack_high_hand, attack_mid_leg, attack_high_leg and attack_low_sweep
to the existing rig.
Do not change the rig, baseline Actions, model or materials.
```

The pipeline should understand this as a surgical animation task and avoid loading/executing unnecessary modeling work.

## Anti-degradation contract

Every meaningful revision checks:

```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

Then:

```text
KEEP
CORRECT
or
REVERT
```

## Deterministic sculpt approach

The sculpt skill uses bounded regional operations:

```text
deform
smooth
inflate / deflate
pinch
crease
flatten / planarize
relax
```

Each operation is described by explicit target, center, radius, strength, falloff and symmetry. This is compatible with a Blender MCP that exposes generic Blender Python execution; dedicated sculpt tools are useful but not required.

## Reference reconstruction principles

For orthographic reference sheets:

```text
Front: lock X/Z and recognizable silhouette.
Side: solve Y depth while preserving the front contract.
Back: solve backside structure.
Top: solve X/Y cross-section.
```

Reference conflicts are documented instead of averaged blindly.

## QA

Default geometry evidence:

```text
Front
Back
Left
Right
3/4 Front
3/4 Back
```

Before/after comparisons must keep cameras, lighting, pose, framing and render settings fixed.

## Stickmans Duel isolation

Most of this stack is universal. Stickmans Duel rules are deliberately contained in:
- `Blender_Character_Modeling_SKILL`;
- `Blender_Character_QA_SKILL`;
- `Blender_Character_Rigging_Animation_Godot_SKILL`.

They activate only when the active task is explicitly about Stickmans Duel.

Contained project rules include:
- fixed 18-bone fighter naming contract;
- GLB production format;
- no root motion for production clips;
- protected baseline Actions;
- four additional attack Action names;
- idle / backward-walk / punch / landing requirements;
- cloth-bone-chain approach for live Godot secondary motion.

## Research basis

The stack is an original synthesis of useful workflow ideas from Blender agent tooling and our production experience. It uses specialist routing, source-locked reconstruction, fixed multiview evidence, iterative refinement, manifest-driven validation and deterministic local sculpt operations.

## Recommended smoke tests

### Routing

```text
Inspect this character and report the largest proportion mismatch.
Do not edit anything.
```

### Surgical edit

```text
Make only the head 5% larger.
Preserve eyes, body, rig, Actions and materials.
```

### Sculpt

```text
Increase only the right forearm volume slightly.
Preserve elbow/wrist landmarks.
```

### Animation

```text
Add only attack_high_hand on the existing Stickmans Duel rig.
No rig changes, no root motion.
```

## Versioning suggestion

```text
1.0.x — wording/recipes/bug fixes
1.x.0 — new workflows or new gates
2.0.0 — incompatible skill routing/contracts
```

When a real production failure reveals a reusable lesson:
1. record the evidence;
2. phrase the fix generically;
3. add it to the correct skill;
4. keep arbitrary project facts in the project-specific section;
5. re-run the relevant smoke test.
