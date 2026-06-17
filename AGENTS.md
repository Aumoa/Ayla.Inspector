# Repository Instructions

Policy version: 2026-06-17.5

This repository is a plugin project intended to be attached to a host Unity
project.

## Project Intent

Ayla Unity plugin projects are part of the broader Ayla ecosystem. Prefer
reusable engine-style systems and clear Unity integration points over one-off
scene-local behavior when the plugin scope implies a shared API.

## Design Principles

Prefer faithful object-oriented design by default, with clear responsibilities,
encapsulation, and extension points.

Performance-critical code may deliberately bend object-oriented design when
doing so improves performance without significantly harming readability,
maintainability, or local reasoning.

## Local Instructions

If `AGENTS.local.md` exists at the repository root, read it and follow it as
private, machine-local context. Treat it as optional for other contributors and
do not assume it exists outside the current working tree.

Project-local Codex skills belong under `.codex/skills/`. Skill directories
whose names start with `local-` are private local skills and must not be
committed.

Do not copy or synchronize `AGENTS.local.md` or `.codex/skills/local-*` to other
repositories.

## Code Style

Repository files and generated documentation should be written in English.

When creating or editing files, use the line ending appropriate for the current
platform: CRLF on Windows and LF on Linux or macOS.

C# member fields should use the `m_` prefix. Static fields should use the `s_`
prefix when a prefix is needed to avoid naming collisions or match nearby code.

Unless there is a specific reason not to, add `#nullable enable` to C# files so
nullable reference analysis remains effective.

When adding `#nullable enable`, place it at the very top of the C# file, before
any `using` directives or namespace declarations.

## Unity Folder Layout

Unity C# scripts must be created under a `Script` subfolder. Runtime scripts
belong under `Runtime/Script/`, and editor-only scripts belong under
`Editor/Script/`.

Code that is used at runtime must live under `Runtime/`. Code that depends on
UnityEditor or is only used in the Unity editor must live under `Editor/`.

## Ayla Plugin Family

Ayla plugin repositories are expected to live as sibling directories whose names
start with `Ayla.` under the same parent directory, for example:

```text
<Parent>/Ayla.Core
<Parent>/Ayla.RemoteDebug
```

The parent directory itself is not fixed and may differ between machines or host
Unity projects.

When editing shared `AGENTS.md`, shared `.gitignore` rules for agent-local files,
or shared `.codex/skills/*` entries, inspect sibling `Ayla.*` repositories under
the same parent directory. If equivalent shared instructions or shared skills are
missing or outdated, apply the same update there too, as long as filesystem and
user permissions allow it.

Treat each sibling `Ayla.*` directory as a separate Git boundary. Check its Git
status before changing it. Do not revert unrelated changes. Do not commit in a
sibling repository unless the user has explicitly allowed commits for that
repository. Never push any repository without explicit user approval.

When shared instructions conflict between Ayla repositories, prefer the rule with
the newest explicit policy version. If no version makes the newer rule clear,
prefer the rule that is more valid for the current project. If that is still
ambiguous, ask the user how to resolve the conflict.

## Unity Host Project

This plugin needs a host Unity project for normal Unity-based testing,
validation, and integration work.

When adding or changing functionality, use a test Unity project that covers the
behavior being changed, and place this plugin under that project's hierarchy.
The recommended location is:

```text
<UnityProject>/Assets/Plugins/<PluginDirectory>
```

Use the repository-local `ayla-unity-validation` skill at
`.codex/skills/ayla-unity-validation/SKILL.md` for detailed Unity host-project
validation, test selection, and reporting workflows.

Use the repository-local `ayla-unity-pr-review` skill at
`.codex/skills/ayla-unity-pr-review/SKILL.md` for pull request reviews, branch
diff reviews, architecture checks, and merge-readiness checks.

## Pull Request Review Policy

Use the current ordinary GitHub user account when submitting pull request review
feedback that evaluates code, opens new review findings, approves, or requests
changes.

Do not use GitHub App, bot, or integration credentials for review feedback that
should appear as the current user's reviewer judgment. If suitable current-user
credentials are unavailable, report the review findings to the user instead of
posting them remotely.

GitHub App or bot credentials may be used for author-side pull request activity,
such as replying to existing review feedback, explaining pushed commits,
updating pull request descriptions, reporting validation results, or asking for
re-review after the user requests that workflow.

Before approving a pull request, understand the intent of the changed code.
Infer intent from the source, tests, pull request description, names, structure,
comments, and surrounding implementation. If the intent cannot be inferred, ask
the pull request author to explain it.

Treat unresolved intent uncertainty as a merge-readiness blocker, and do not
approve the pull request until the author explains the intent or the code is
clarified enough to review its behavior.

## GitHub CLI And Shared-State Safety

Codex may use the `gh` command for read-only GitHub inspection when it is
available. Use the repository-local `ayla-unity-validation` and
`ayla-unity-pr-review` skills for detailed validation and review workflows.

Before choosing credentials for GitHub writes, remote branch publication, pull
request creation or updates, author-side pull request responses, or remote
validation, read and follow
`.codex/skills/github-app-credential-policy/SKILL.md`.

Codex may perform local-only actions at its own discretion when otherwise
allowed by these instructions, including read-only checks, local commits, and
local merges.

Treat `dev`, `master`, `main`, release branches, production branches, and any
branch or environment used by other users as protected shared targets. Always
get final user approval before operations that publish, push, deploy, release,
upload, create or update remote pull requests, or otherwise change online state
on protected shared targets or production-like environments.

Branches that are clearly isolated work branches, especially branches whose
names start with `codex/`, may use a more flexible approval model for pushing,
draft pull request updates, and other collaboration or validation tasks when
doing so is useful for the requested work.

Destructive or broad shared-state actions still require explicit user approval.
This includes force-pushing, deleting remote branches or resources, publishing
or deploying to shared environments, or changing protected shared targets. If
Codex is unsure whether a branch is isolated, treat it as shared.

If a GitHub write has a required actor and receives a 401, 403, or permission
error, do not retry with a different actor. Diagnose and report the credential
or permission mismatch.

Before merging a task branch into a protected shared branch, remove branch-local
CI, GitHub Actions, branch trigger, permission, environment, generated file, or
validation-only configuration changes that would affect the protected branch.
Permanent CI or GitHub Actions policy changes are normal repository policy
changes and must not be treated as temporary branch-local validation settings.

## Instruction Storage

Keep short, always-on repository policies in `AGENTS.md`.

When a requested persistent instruction is mainly useful for a recurring
workflow, verification procedure, tool usage pattern, or detailed domain guide,
create or update a repository-local skill under `.codex/skills/<skill-name>/`
and link it from `AGENTS.md`.

Prefer scripts only for deterministic, repeatedly executed operations where a
command or code snippet would otherwise be rewritten often.

Do not split instructions just to split them. Keep compact universal rules in
`AGENTS.md` when a skill or script would not reduce future context or execution
risk.

## Commit Rules

When implementing a requested feature or fix, split the work into meaningful
feature-sized commits. Create one commit per independently reviewable functional
unit.

Do not mix unrelated refactors, formatting, dependency updates, or bug fixes into
the same commit unless they are required for that feature.

Before committing, run the relevant build or test command when it is known and
practical. If the relevant build or tests fail, do not commit until the failure
is fixed or the user explicitly asks to commit anyway. If validation cannot be
run, mention that in the final response.

Unless the user explicitly asks not to commit, create the commit after requested
changes pass the relevant build or tests. Do not commit user-made unrelated
changes. If the working tree already contains unrelated changes, isolate only
Codex-made changes in the commit. If a clean feature-sized commit is not
possible, stop and explain why.

Commit subjects must use this exact format unless the user asks for a different
message format:

```text
Codex: Commit Message
```

Replace `Commit Message` with a concise imperative summary of the feature or
change. Keep the subject under 72 characters when practical.

AI-generated commit titles must be written in English. When practical, include a
concise commit message body with 1-3 bullet points describing what changed and
any validation result.

For commits intended to participate in a GitHub App-authenticated isolated
work-branch workflow, follow
`.codex/skills/github-app-credential-policy/SKILL.md` before choosing the commit
author and committer identity.
