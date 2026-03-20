## 🐾 Release Mascot

<!-- MASCOT -->
![Release Mascot](mascots/1.0.0.png)
<!-- /MASCOT -->




A GitHub Action that generates a unique mascot for every release.

## Usage
Create a new action or add to your current release pipeline. Here is an example Action which I store in `.github/workflows/generateMascot.yml`

```yaml
name: Generate Mascot
 
on:
  release:
    types: [published]
 
permissions:
  contents: write
 
jobs:
  generate-mascot:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          ref: master
          fetch-depth: 0
 
      - name: Generate Release Mascot
        uses: OwainWilliams/OC.ReleaseMascot@main
        with:
          Tag: ${{ github.event.release.tag_name }}
 
      - name: Commit mascot
        run: |
          git config user.name  "github-actions[bot]"
          git config user.email "github-actions[bot]@users.noreply.github.com"
          git add mascots/
          git commit -m "chore: add mascot for ${{ github.event.release.tag_name }}"
          git push
```

You may need to change `ref:master` if you have a different branch name e.g. `ref:main` 
Other than that, this will create a mascot within a mascot folder on the root of your repo. 

You could then amend your actions to automatically update your `Readme` file or another file within your repo to display your mascot. 

Examples of how to do this can be found in the `/scripts` folder in this repo

---

## 🏆 Mascot Hall of Fame

<!-- MASCOT_HALL_OF_FAME -->
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

