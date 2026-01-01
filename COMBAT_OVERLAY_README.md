# Combat Overlay - Name Plates

A simple, clean overlay for OBS that displays player name plates for both teams in Echo Arena/Echo VR matches.

## Features

- **Player Name Plates**: Shows all players from both teams with their names and numbers
- **Team Colors**: Orange team on the left (orange gradient), Blue team on the right (blue gradient)
- **Possession Indicator**: When a player has disc possession:
  - Name plate glows and scales up slightly
  - White indicator dot pulses next to their name
- **Stunned Status**: When a player is stunned:
  - Name plate becomes semi-transparent and grayscaled
  - "STUNNED" badge appears on the name plate
- **Smooth Animations**: Fade-in animations when players join, smooth transitions for state changes
- **Transparent Background**: Works perfectly as an OBS browser source

## How to Use in OBS

1. **Start Spark** - Make sure Spark is running and connected to Echo VR
2. **Add Browser Source in OBS**:
   - Add a new "Browser" source
   - Set the URL to: `http://localhost:6724/combat_overlay`
   - Set Width: `1920`
   - Set Height: `1080`
   - Check "Shutdown source when not visible" (optional for performance)
   - Check "Refresh browser when scene becomes active" (optional)
3. **Position the overlay** over your gameplay capture
4. **Done!** The name plates will automatically update in real-time

## Alternative Access

You can also access the overlay directly in your browser at:
- `http://localhost:6724/combat_overlay` (Svelte version - recommended)
- `http://localhost:6724/combat_overlay.html` (Static HTML version)

## Customization

The overlay is designed to be simple and clean. If you want to customize it:

### Colors
- Orange team: `#ff9627` with gradient to `#bd7a24`
- Blue team: `#4890dd` with gradient to `#3f6ae1`

### Size
- Name plates are `200px` minimum width
- Font size is `20px` for names, `16px` for numbers

### Position
- Orange team: Left side, top to bottom
- Blue team: Right side, top to bottom
- Both teams start 20px from the edge

You can edit the files to change these values:
- Svelte version: `Overlay/src/routes/(overlays)/combat_overlay/+page.svelte`
- HTML version: `Overlay/static/combat_overlay.html`

## Technical Details

The overlay connects to Spark's WebSocket server and subscribes to the `frame_10hz` event, which provides real-time game state updates 10 times per second. It displays:

- Player name (`player.name`)
- Player number (`player.number`)
- Possession state (`player.possession`)
- Stunned state (`player.stunned`)

The overlay automatically handles players joining/leaving and updates all states in real-time.

## Troubleshooting

**Overlay doesn't show anything:**
- Make sure Spark is running
- Make sure you're in an Echo VR match (or connected to the API)
- Check that the URL is correct in OBS
- Try refreshing the browser source

**Players not updating:**
- Make sure Echo VR's API is enabled (it should be by default)
- Check Spark's connection to Echo VR
- Try restarting the overlay browser source in OBS

**Performance issues:**
- Enable "Shutdown source when not visible" in OBS browser source settings
- Make sure you're not running too many overlays simultaneously
