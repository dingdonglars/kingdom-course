# M4 milestone checklist

## Wins ritual
- [ ] **Refresh the README at the repo root** — re-walk the four sections from M0.4. The repo is now a deployed web API with OAuth and multi-user persistence; the *How to run* section needs the live URL + the `dotnet user-secrets` step, and *What I learned* gets a Phase 3 paragraph.
- [ ] Write the M4 entry in `journal/wins.md`
- [ ] Take a screenshot of your live URL + Scalar (`/scalar/v1`)
- [ ] Post the screenshot + before/after one-liner to `#wins`
- [ ] Tag locally: `git tag m4-phase-3-complete && git push origin m4-phase-3-complete`

## AI Unlock (THE BIG ONE)
- [ ] Edit `CLAUDE.md` line 7 in **your repo** — change `pre-unlock` → `post-unlock`
- [ ] Commit: `git commit -am "[M4] AI Unlock — flip mode pre-unlock → post-unlock"`
- [ ] Push
- [ ] Re-read your own `CLAUDE.md` end-to-end with the new mode in mind
- [ ] Update your PR template (or copy from the post-unlock template in `STANDARDS.md`) to include the AI-assistance section

## Mentor PR
- [ ] Open the M4 PR (`phase-3 → main`) on github.com — banner *"Compare & pull request"* (or *Pull requests → New pull request*)
- [ ] Title: `M4 — Phase 3 — Live API`; body has wins bullets + the AI-assistance section (empty for this PR — first post-unlock PR is symbolic) + `**Reviewer:** @dingdonglars`
- [ ] Mentor reviews → Approves → you Merge → delete the `phase-3` branch
- [ ] Locally: `git switch main && git pull`
- [ ] Viva session scheduled

## Mentally
- [ ] Pause for 10 minutes. You shipped real software to the internet. **That is a legitimately big deal.**
