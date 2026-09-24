# Changelog

## 0.3.3 - 2026-09-23

Initial public beta release.

- Preserves item styles and upgrade visuals on item stands.
- Supports dedicated-server installation without requiring the mod on clients.
- Supports client installation for singleplayer and unmodded servers.
- Clears visual metadata when a stand becomes empty to prevent stale styles or
  quality values from affecting later items.
- Prioritizes corrected stand data for immediate network delivery, preventing
  delayed visual reversion and duplicate placement effects.
- Verifies the attached item matches the queued item before writing client-side
  metadata, preventing simultaneous placements from crossing item values.
