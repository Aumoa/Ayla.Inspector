---
name: ayla-unity-pr-review
description: Repository-local pull request and branch-diff review guidance for Ayla Unity plugin projects. Use when Codex reviews PRs, branch diffs, CI failures, Unity integration changes, asmdef changes, architecture changes, class responsibilities, duplication, tests, or merge readiness in Ayla.* repositories.
---

# Ayla Unity PR Review

## Overview

Ayla Unity plugin reviews should protect correctness, Unity integration health,
and the repository's intended architecture without turning style preference into
noise.

## Review Priorities

- Lead with actionable findings ordered by severity.
- Ground findings in concrete source locations, logs, commands, diffs, or Unity test results.
- Separate blockers from residual risks and optional follow-up ideas.
- Understand the author's intended behavior and design before approving a pull request.
- If the intent of changed code cannot be inferred from names, structure, comments, tests, pull request description, or surrounding implementation, ask the pull request author to explain it.
- Treat unresolved intent uncertainty as a merge-readiness blocker. Do not approve until the author explains the intent or the code is clarified enough to review its behavior.
- Connect Unity-specific findings back to asmdef boundaries, Editor/Runtime separation, serialization, lifecycle behavior, package placement, or host-project validation.

## GitHub Actions And Remote Review Safety

- Use `gh` to inspect Actions runs, jobs, and logs when the command is available and read-only GitHub inspection would improve confidence.
- If `gh` or Actions access is unavailable, state that Actions validation was skipped and perform a stricter source and Unity workflow review.
- Before using GitHub credentials for remote validation, pull request updates, or bot-authored review responses, follow `.codex/skills/github-app-credential-policy/SKILL.md`.
- On protected shared branches such as `dev`, `master`, `main`, release branches, or branches used directly by other people, request explicit user approval before shared-state actions.
- On clearly isolated task branches, especially branches whose names start with `codex/`, non-destructive shared-state actions may be performed when they support the review or validation work.
- Destructive or broad shared-state actions still require explicit user approval. If branch ownership is unclear, treat the branch as shared.

## Unity Plugin Checks

- Verify runtime assemblies do not depend on `UnityEditor` or editor-only code.
- Verify C# scripts are placed under `Runtime/Script/` for runtime code or `Editor/Script/` for editor-only code.
- Check asmdef references, optional package dependencies, platform constraints, and define constraints when touched.
- Check Unity serialization behavior for renamed fields, private serialized fields, `[SerializeReference]`, asset GUID assumptions, and domain reload behavior.
- Review MonoBehaviour, ScriptableObject, async, cancellation, pooling, and disposal code for lifecycle edge cases.
- Ensure tests or validation cover the host Unity project integration when behavior depends on Unity runtime or editor behavior.

## Object-Oriented Design

- Prefer object-oriented designs with clear responsibilities, encapsulation, and extensibility unless the touched code is genuinely performance-critical.
- Accept performance-oriented departures from object-oriented design only when they do not significantly harm readability, maintainability, or local reasoning.
- Use Microsoft's recommended C# design guidelines and object-oriented patterns from major engines as references, adapted to Ayla's Unity plugin conventions.

## Class Responsibility

- Check whether each meaningful feature is owned by a clear class or collaborator.
- Flag classes that combine unrelated responsibilities when that coupling makes behavior harder to extend, test, or review.
- Prefer cohesive feature units over scattered special cases.
- Do not request extra splitting when the existing responsibility boundary is already understandable and further separation would mostly add indirection.

## Duplication

- Flag duplicated code when it represents the same feature, policy, decision, Unity integration behavior, or platform behavior and merging it would reduce bug risk or future maintenance.
- Prefer one cohesive implementation for repeated behavior that must evolve together.
- Avoid demanding abstractions for small incidental duplication when the abstraction would be noisier than the repeated code.

## Merge Readiness

- Before approving or merging into protected shared branches, check whether the change includes CI, GitHub Actions, branch trigger, permission, environment, generated file, or validation-only configuration changes that were only needed for task-branch validation.
- Temporary branch-local CI settings are allowed on isolated task branches, but they must be removed before the task branch is approved for merge into a protected shared branch.
- Do not block intentionally permanent CI policy changes merely because they affect the protected branch after merge.
- Treat unremoved temporary CI or validation settings as a merge blocker when they would affect protected branches.
- Clearly state whether the change is safe to merge, needs fixes first, or needs CI/runtime validation before judgment.

## Review Comments

- Write external PR review comments in English unless the user asks otherwise.
- Explain the interpretation and recommendation to the user in Korean when the surrounding conversation is Korean.
- Submit review feedback, approvals, change requests, and independent code-evaluation comments only with the current ordinary GitHub user account.
- Use GitHub App or bot credentials only for author-side pull request responses, such as replying to existing review feedback, explaining pushed commits, reporting validation results, or asking for re-review when requested.
