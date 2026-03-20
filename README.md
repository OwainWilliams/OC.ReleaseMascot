<div align="center">

# 🐾 OC.ReleaseMascot

**Give every release a face.**

A GitHub Action that automatically generates a unique pixel-art mascot, assigns it a Pokémon-style name, and logs it to your README — triggered every time you publish a release.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?logo=githubactions&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)

</div>

---

## ✨ What It Does

Every time you publish a release, this action automatically:

| | What happens |
|---|---|
| 🎨 | Generates a unique pixel-art creature (slime, ghost, dragon, imp, beast, or winged) with randomised colours |
| 🏷️ | Gives it a Pokémon-style name — e.g. `Voltzeneon`, `Glaciryuara`, `Temprazelchu` |
| 📖 | Prepends a new entry to your README Hall of Fame with the version, name, and image |
| 💾 | Commits and pushes everything back to your repo automatically |

No configuration. No secrets. No manual steps after setup.

---

## 🚀 Setup — Three Steps

### Step 1 — Add the workflow

Create `.github/workflows/release-mascot.yml` in your repo:

```yaml
name: Release Mascot

on:
  release:
    types: [published]

permissions:
  contents: write

jobs:
  mascot:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          ref: main        # change to 'master' if that's your default branch
          fetch-depth: 0

      - name: Generate Release Mascot
        uses: OwainWilliams/OC.ReleaseMascot@main
        with:
          Tag: ${{ github.event.release.tag_name }}
```

> 💡 Change `ref: main` to `ref: master` (or whatever your default branch is called) if needed.

---

### Step 2 — Add the marker to your README

Add the following section to your `README.md` wherever you want the Hall of Fame to appear:

```markdown
## 🏆 Mascot Hall of Fame

<!-- MASCOT_HALL_OF_FAME -->
```

> ⚠️ The `<!-- MASCOT_HALL_OF_FAME -->` comment is **required**. The action uses it to know where to inject new entries. It is invisible to readers on GitHub — it's just a marker. Without it, the README update step will fail.

---

### Step 3 — Publish a release

Go to your repository on GitHub → **Releases** → **Draft a new release** → create a tag (e.g. `1.0.0`) → **Publish release**.

The action fires automatically. Within a minute, your first mascot will be committed to your repo. 🎉

---

## 📁 What Gets Added to Your Repo

After each release the action commits two changes:

```
your-repo/
├── mascots/
│   └── 1.0.0.png       ← pixel-art mascot, named after your release tag
└── README.md            ← updated with the new Hall of Fame entry
```

Each Hall of Fame entry looks like this:

```markdown
### `1.0.0` — Voltzeneon

![Voltzeneon (1.0.0)](mascots/1.0.0.png)

---
```

The most recent release always appears at the top.

---

## ⚙️ How It Works Under the Hood

The action runs four steps, all handled automatically by `action.yml`:

**1. Generate the mascot image**
Uses [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) to draw a 32×32 pixel-art creature, scaled up to 256×256. The creature type and colours are randomly selected on each run.

**2. Generate the mascot name**
Fuses random syllable chunks (prefix + middle + suffix) to produce Pokémon-style names. The name is seeded by the release tag, so the same tag always produces the same name — reruns are safe.

**3. Update the README**
Finds the `<!-- MASCOT_HALL_OF_FAME -->` marker in your README and injects the new entry immediately after it.

**4. Commit and push**
Commits the new image and updated README with a message like:
```
chore: add mascot for 1.0.0 — Voltzeneon
```
Pushes to whichever branch was checked out — works with `main`, `master`, or any other default branch name.

---

## 🔑 Permissions

The workflow needs write access to push the mascot commit back to your repo. This is handled by the `permissions` block in the workflow file — no tokens or secrets need to be configured manually.

```yaml
permissions:
  contents: write
```

---

## 🏆 Mascot Hall of Fame

<!-- MASCOT_HALL_OF_FAME -->

---

## ❓ Troubleshooting

**`marker not found` error**
The `<!-- MASCOT_HALL_OF_FAME -->` comment is missing from your `README.md`. Add it exactly as shown — correct capitalisation, no extra spaces.

**Tag name is blank in the logs**
Your workflow is not triggered by a release event. Make sure you are using `on: release: types: [published]` — triggers like `push` or `workflow_dispatch` do not populate `github.event.release.tag_name`.

**Push rejected / non-fast-forward error**
You have a commit step in your calling workflow as well as using this action. The action already commits and pushes — remove any `git commit` / `git push` steps from your own workflow.

**Wrong branch being pushed to**
Make sure the `ref:` value in your `actions/checkout` step matches your actual default branch name (`main` or `master`).

---

## 📄 Licence

MIT — use it, fork it, build on it.

---

<div align="center">

Built with way too much enthusiasm by [Owain Williams](https://github.com/OwainWilliams)

</div>


## 🏆 Mascot Hall of Fame

<!-- MASCOT_HALL_OF_FAME -->
### `2.0.1` — Sylvanatoth

![Sylvanatoth (2.0.1)](mascots/2.0.1.png)

---
### `2.0.0` — Frostshiyx

![Frostshiyx (2.0.0)](mascots/2.0.0.png)

---
### `1.3.0-alpha001` — Chronorexeon

![Chronorexeon (1.3.0-alpha001)](mascots/1.3.0-alpha001.png)

---
### `1.2.1-alpha001` — Tempravoel

![Tempravoel (1.2.1-alpha001)](mascots/1.2.1-alpha001.png)

---
### `1.2.0` — Lumilixara

![Lumilixara (1.2.0)](mascots/1.2.0.png)

---
### `1.0.1-beta006` — Terramiaki

![Terramiaki (1.0.1-beta006)](mascots/1.0.1-beta006.png)

---

### `1.0.0` — Admiral Fluffy Pawsworth

![Admiral Fluffy Pawsworth (1.0.0)](mascots/1.0.0.png)

---

