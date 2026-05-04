# tools/

Authoring helpers used during course-content authoring. Not part of the learner-facing materials.

## screenshot.ps1

Captures browser screenshots from the reference repo at a tagged commit. Used during Phase 4 authoring to generate `screenshots/` for browser-UI lessons.

**Status:** skeleton at Plan 0 time. Filled in during p006 authoring when browser screenshots are first needed.

**Usage (target shape):**

```powershell
.\tools\screenshot.ps1 -ReferenceTag phase-4-complete -OutputDir phase-4-browser-ui\4.7-componentized-ui\screenshots
```

The script (when implemented):
1. Clones / pulls `kingdom-reference` into a temp dir.
2. Checks out the named tag.
3. Builds and starts the reference app.
4. Uses Playwright headless Chromium to capture each named view.
5. Compresses to JPEG (quality 70, max width 1200px) per spec §20.
6. Copies into the named output dir.
