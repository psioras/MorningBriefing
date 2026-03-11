#!/bin/bash
set -e

echo "Heartbeat script starting..."

# Configure git identity for the commit
git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"

# Update the timestamp file
echo "Last heartbeat: $(date -u '+%Y-%m-%d %H:%M:%S UTC')" > heartbeat.txt

# Stage, commit and push
git add heartbeat.txt
git commit -m "heartbeat: $(date -u '+%Y-%m-%d')"
git push

echo "Heartbeat done!"
