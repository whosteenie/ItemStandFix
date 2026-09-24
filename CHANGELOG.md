# Changelog

## 0.3.4 - 2026-09-23

- Prevents transient server deserialization states from clearing metadata on
  occupied item stands.
- Limits empty-stand metadata cleanup to confirmed local item-removal events;
  stale metadata is safely replaced on the next placement.

## 0.3.3 - 2026-09-23

Initial public beta release.

- Preserves item styles and upgrade visuals on item stands.
- Supports dedicated-server installation without requiring the mod on clients.
- Supports client installation for singleplayer and unmodded servers.
- Clears visual metadata from confirmed local item-removal events and replaces
  stale metadata whenever another item is placed.
- Prioritizes corrected stand data for immediate network delivery, reducing
  delayed visual reversion.
- Verifies the attached item matches the queued item before writing client-side
  metadata, preventing simultaneous placements from crossing item values.
