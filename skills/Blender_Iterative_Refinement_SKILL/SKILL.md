---
name: Blender Iterative Refinement
version: 1.0
description: Bounded evidence-driven improvement loop for Blender characters. Freezes a candidate, ranks visible errors, changes one error class at a time, rechecks identical evidence, and keeps/corrects/reverts based on net quality.
when_to_use: Use when output is subpar, the user asks to improve quality, a repeated defect persists, reference matching is failing, or a second controlled polish pass is needed.
---

# Blender Iterative Refinement Skill

## 0. Purpose

Refinement is not random polishing.

Use:
```text
FREEZE
→ MEASURE
→ RANK
→ FIX ONE ERROR CLASS
→ RECHECK
→ KEEP/CORRECT/REVERT
→ NEXT
```

The loop exists to prevent regressions and blind retries.

## 1. Freeze the candidate

Record:
- source `.blend`;
- target objects;
- current transforms;
- current object hierarchy;
- topology state;
- materials;
- armature/Action state if present;
- fixed-view evidence;
- task requirements;
- protected invariants.

Do not overwrite the only recoverable candidate.

Follow the project's backup policy instead of creating unlimited copies.

## 2. Build an error inventory

Classify observed problems:

```text
REFERENCE_INTERPRETATION
SILHOUETTE
GLOBAL_PROPORTION
LOCAL_PROPORTION
DEPTH
PART_COUNT
STRUCTURAL_CONNECTION
SCULPT_FORM
TOPOLOGY
DEFORMATION
WEIGHTS
RIG
ANIMATION
MATERIAL
EXPORT
PRESENTATION
```

Separate actual defects from preferences outside scope.

## 3. Rank errors

Default impact order:

```text
identity/reference
> silhouette
> global proportion
> structural/deformation
> topology blockers
> secondary form
> material
> tertiary detail
```

Add:
- user priority;
- gameplay visibility;
- downstream risk;
- repair cost.

Fix the highest-value error, not the easiest one.

## 4. One error class per iteration

An iteration should have a narrow hypothesis.

Bad:
```text
fix head, arms, boots, material, lighting, rig
```

Good:
```text
Hypothesis:
Head reads too small in front and 3/4.
Change:
Increase head shell X/Z scale 6%, preserve Y depth and eye placement ratio.
Validation:
front/side/3/4 fixed cameras.
```

Multiple tightly coupled changes are allowed only when they solve one root cause.

## 5. Before/after evidence

Use identical:
- cameras;
- lighting;
- render settings;
- pose;
- frame;
- material state.

Do not "win" a comparison by changing presentation.

## 6. Anti-degradation checklist

After every meaningful iteration:

```text
Did requested feature improve?
Did silhouette become worse?
Did another view become worse?
Did topology become worse?
Did deformation zones become worse?
```

Also compare protected invariants.

## 7. Decision matrix

### KEEP
Requested metric/read improved and no meaningful regression.

### CORRECT
Main change is right but caused a bounded regression that can be repaired without invalidating the improvement.

### REVERT
Use when:
- requested issue did not improve;
- net quality is worse;
- protected invariant broke;
- fix created a more severe defect;
- the method is clearly wrong.

Sunk effort is not evidence.

## 8. Iteration budget

Default:
- up to 3 focused iterations for one defect family;
- after 2 ineffective attempts with the same method, switch method;
- after 3 without progress, diagnose missing information/tooling rather than brute forcing.

The user may authorize a longer refinement session.

## 9. Method switching

Examples:

```text
manual vertex nudging
→ proportional / lattice / landmark correction

brush sculpt
→ deterministic region sculpt

surface patch repair
→ local retopology rebuild

generic visual guess
→ reference overlay / landmark measurements

weight painting only
→ inspect bone placement + topology
```

## 10. Repair ownership

Route by diagnosis:

- reference/landmark → Reference Reconstruction;
- primary mesh → Character Modeling;
- organic volume → Organic Sculpting;
- edge flow/deformation → Retopology & Deformation;
- rig/Action/export → Rigging/Animation/Godot.

QA owns verification, not repair.

## 11. Preserve approved areas

Create a protected list.

Example:
```text
APPROVED:
- eye shape
- headband height
- torso width
- left boot
- bone names
- baseline animations
```

A repair to one area must not casually alter these.

## 12. Visual evidence cadence

For geometry:
- front;
- side;
- 3/4.

For symmetry:
- front;
- back.

For depth:
- side;
- top/3/4.

For deformation:
- relevant extreme pose + neutral.

For animation:
- contact sheet / key extremes.

Avoid rendering six views after every microscopic vertex move; match evidence cost to change scale.

## 13. Difference logging

Each iteration records:

```text
ITERATION:
DEFECT:
HYPOTHESIS:
CHANGE:
EXPECTED RESULT:
ACTUAL RESULT:
REGRESSIONS:
DECISION:
NEXT:
```

This prevents cycling through the same failed idea.

## 14. Skill-learning boundary

A repeated failure may reveal a missing reusable rule.

Do:
- formulate the lesson generically;
- avoid embedding one character's arbitrary dimensions into a universal skill;
- add project-specific facts only to a project profile/section.

Do not automatically rewrite the installed skill stack during ordinary modeling unless the user asks to evolve the skills.

## 15. Stop conditions

Stop when:
- explicit user issue is solved;
- acceptance gate passes;
- additional changes are minor/out of scope;
- next improvement requires user art direction;
- no safe improvement remains with current evidence.

## 16. Final refinement report

```text
Improved:
- ...

Preserved:
- ...

Rejected/Reverted attempts:
- ... (only if useful)

QA:
- PASS/FAIL

Suggestions not executed:
- ...
```
