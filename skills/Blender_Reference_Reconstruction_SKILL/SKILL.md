---
name: Blender Reference Reconstruction
version: 1.0
description: Source-locked reference analysis and reconstruction skill for characters from front/side/back/three-quarter sheets, concept art, orthographic turnarounds, or mixed visual references.
when_to_use: Use when visual fidelity to supplied references matters, when modeling from turnaround sheets, or when an existing model fails to match the reference.
---

# Blender Reference Reconstruction Skill

## 0. Core rule

The reference is evidence, not decoration.

Do not build a plausible character from memory and later "make it look closer". First extract the visual contract, then model against that contract.

## 1. Classify reference intent

Choose one:

### A. Inspiration
The reference defines:
- style;
- mood;
- broad design language.

Exact proportion matching is not required.

### B. Shape-locked
The reference defines:
- silhouette;
- major proportions;
- visible parts;
- landmark positions.

The model should visibly match.

### C. Orthographic contract
Front/side/back/top views define measurable geometry.

Treat them as registered projections unless evidence shows the sheet itself is inconsistent.

User statements such as "максимально близко", "точно по референсу", "1:1", "front/side/back" imply B or C.

## 2. Source-of-truth hierarchy

Default:
1. User's explicit correction.
2. Primary/front reference for recognizable silhouette and feature positions.
3. Side reference for depth.
4. Back reference for backside structure.
5. Top reference for cross-section/depth spread.
6. Three-quarter views for volume plausibility.
7. Generic anatomy knowledge only for missing information.

Never let generic anatomy overwrite intentional stylization.

## 3. Build a reference manifest

Before geometry, record:

```text
SUBJECT:
REFERENCE MODE:
PRIMARY VIEW:
KNOWN SCALE:
STRUCTURAL PARTS:
DECORATIVE PARTS:
SYMMETRY:
INTENTIONAL ASYMMETRY:
LANDMARKS:
PROPORTION UNIT:
MATERIAL/COLOR BOUNDARIES:
UNCERTAINTIES:
```

For a humanoid character, useful landmarks include:
- top of head;
- eye line;
- chin;
- shoulder centers;
- clavicle width;
- armpit;
- chest bottom;
- waist;
- pelvis center;
- hip centers;
- elbow centers;
- wrist centers;
- knuckles;
- knee centers;
- ankle centers;
- toe/heel extremes.

## 4. Normalize coordinates

Use a stable subject coordinate system.

Recommended character convention:
- Z = up;
- X = left/right;
- Y = depth;
- origin near world center or pelvis/ground depending on project.

Pick one canonical height:
```text
H_total = 1.0
```
or real-world units if supplied.

Express landmarks as ratios when scale is unknown.

Example:
```text
head_top_z = 1.000
chin_z = 0.865
shoulder_z = 0.790
pelvis_z = 0.505
knee_z = 0.270
ground_z = 0.000
```

The numbers are task-derived, never hardcoded defaults.

## 5. Orthographic registration

When multiple orthographic views exist:

### Front
Lock:
- X;
- Z;
- front silhouette;
- landmark heights.

### Side
Primarily solve:
- Y depth;
- Z agreement;
- front/back contour.

Do not change front-view X coordinates merely to improve the side view unless the references prove front registration was wrong.

### Back
Solve:
- backside geometry;
- rear silhouette;
- hidden accessories;
- attachment structure.

Do not damage the locked front read.

### Top
Solve:
- X/Y cross-section;
- shoulder/chest/pelvis depth relationships;
- accessory placement.

## 6. Silhouette-first reconstruction

Before surface detail, verify masks/silhouettes.

For each primary view:
1. render subject as a flat high-contrast silhouette;
2. compare against the reference;
3. identify largest contour error;
4. fix primary form;
5. repeat.

Useful numerical measurements when tooling is available:
- bounding-box width/height;
- subject center offset;
- key landmark error;
- silhouette overlap / IoU;
- cross-section width ratios.

Metrics are gates and diagnostics, not substitutes for visual inspection.

## 7. Feature-position contract

For eyes, mouth, masks, belts, armor panels, seams, etc., record:
- center position;
- width;
- height;
- angle;
- relative offset to landmarks.

Avoid "looks about right" when the reference supplies a measurable location.

## 8. Character proportion table

Create a compact table before secondary detail.

Example fields:
```text
total_height
head_height
head_width
shoulder_width
chest_width
waist_width
pelvis_width
upper_arm_length
forearm_length
hand_length
thigh_length
shin_length
foot_length
body_depth
head_depth
```

Values should come from the supplied reference or explicit project rules.

## 9. Contradictory references

If views disagree:
1. determine whether perspective distortion caused the mismatch;
2. privilege the designated primary view for recognizability;
3. preserve shared landmark heights;
4. use three-quarter art to resolve volume;
5. record the compromise.

Do not average incompatible views blindly.

## 10. Perspective concept art

For non-orthographic art:
- estimate camera direction;
- separate perspective distortion from anatomy;
- use multiple landmarks, not one apparent length;
- treat occluded dimensions as uncertain;
- use generic anatomy only to interpolate missing structure.

If a single image is insufficient for an exact hidden surface, model a conservative plausible solution and report that it was inferred.

## 11. Reference objects in Blender

Reference images:
- keep in a dedicated `REF_` collection;
- name semantically;
- lock transforms after registration;
- make them non-rendering/export-excluded;
- use controlled opacity;
- align to axes for true orthographic views.

Never accidentally export reference planes or empties.

## 12. Blockout acceptance gate

Before secondary modeling, confirm:

```text
[ ] overall height correct
[ ] head/body ratio correct
[ ] shoulder/hip relationship correct
[ ] limb segment ratios correct
[ ] hands/feet scale correct
[ ] front silhouette acceptable
[ ] side silhouette acceptable
[ ] back structural count correct
[ ] no missing major accessory
```

If any critical item fails, stay in blockout/primary-form stage.

## 13. Handoff to Character Modeling

Provide:

```text
REFERENCE_MODE:
LOCKED_VIEWS:
PROPORTION_TABLE:
LANDMARK_TABLE:
STRUCTURAL_PARTS:
DECORATIVE_PARTS:
SYMMETRY_RULE:
INTENTIONAL_ASYMMETRY:
UNRESOLVED_AMBIGUITIES:
```

The modeling skill must not reinterpret these without evidence.

## 14. Reference-mismatch repair mode

When a model already exists:
1. do not rebuild immediately;
2. render fixed front/side/back;
3. overlay or compare;
4. classify each mismatch:
   - global scale;
   - landmark;
   - silhouette;
   - depth;
   - part count;
   - feature placement;
5. rank by visible impact;
6. repair highest-impact class first;
7. run anti-degradation checks.

## 15. Exit criteria

Reference reconstruction is complete when:
- primary identity reads correctly;
- structural part count matches;
- major landmarks match within the precision justified by the source;
- front and side silhouettes no longer have obvious macro mismatch;
- remaining errors are secondary/tertiary and can be handled by modeling/sculpt/refinement.
