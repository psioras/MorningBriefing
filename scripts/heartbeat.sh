#!/usr/bin/env bash

set -euo pipefail

# Write UTC timestamp to heartbeat.txt
date -u +"%Y-%m-%dT%H:%M:%SZ" > heartbeat.txt

# Configure git user for the action
git config user.name "github-actions[bot]"
git config user.email "41898282+github-actions[bot]@users.noreply.github.com"
git add heartbeat.txt

# commit only if changed
git commit -m "chore(heartbeat): update timestamp $(date -u +%Y%m%dT%H%M%SZ)" || echo "No changes to commit"

# Push to the heartbeat branch (create it if doesn't exist)
git push origin HEAD:refs/heads/heartbeat --no-verify || echo "Push failed"
