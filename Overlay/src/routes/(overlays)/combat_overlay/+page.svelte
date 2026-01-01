<script lang="ts">
	import { SparkWebsocket } from '$lib/js/spark_websocket.js';
	import { onDestroy } from 'svelte';
	import type { Frame, Player } from '$lib/js/Frame';

	let frame: Frame | null = null;
	let sw = new SparkWebsocket();

	sw.subscribe('frame_10hz', (data: Frame) => {
		frame = data;
	});

	onDestroy(() => sw.close());
</script>

<svelte:head>
	<title>Combat Overlay - Name Plates</title>
</svelte:head>

<div id="combat_container">
	{#if frame && frame.teams}
		<!-- Orange Team -->
		<div class="team_container orange">
			{#if frame.teams[1] && frame.teams[1].players}
				{#each frame.teams[1].players as player}
					<div
						class="nameplate orange"
						class:possession={player.possession}
						class:stunned={player.stunned}
					>
						<div class="possession_indicator" />
						<div class="player_name">{player.name}</div>
						<div class="player_number">#{player.number}</div>
						{#if player.stunned}
							<div class="stunned_indicator">STUNNED</div>
						{/if}
					</div>
				{/each}
			{/if}
		</div>

		<!-- Blue Team -->
		<div class="team_container blue">
			{#if frame.teams[0] && frame.teams[0].players}
				{#each frame.teams[0].players as player}
					<div
						class="nameplate blue"
						class:possession={player.possession}
						class:stunned={player.stunned}
					>
						<div class="player_number">#{player.number}</div>
						<div class="player_name">{player.name}</div>
						<div class="possession_indicator" />
						{#if player.stunned}
							<div class="stunned_indicator">STUNNED</div>
						{/if}
					</div>
				{/each}
			{/if}
		</div>
	{/if}
</div>

<style>
	:global(body) {
		width: 1920px;
		height: 1080px;
		overflow: hidden;
		background: transparent;
		margin: 0;
		padding: 0;
	}

	#combat_container {
		position: absolute;
		width: 100%;
		height: 100%;
		display: flex;
		justify-content: space-between;
		padding: 20px;
	}

	.team_container {
		display: flex;
		flex-direction: column;
		gap: 10px;
		max-width: 400px;
	}

	.team_container.orange {
		align-items: flex-start;
	}

	.team_container.blue {
		align-items: flex-end;
	}

	.nameplate {
		display: flex;
		align-items: center;
		padding: 12px 20px;
		border-radius: 8px;
		font-size: 20px;
		font-weight: bold;
		text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
		backdrop-filter: blur(4px);
		transition: all 0.3s ease;
		min-width: 200px;
		position: relative;
		opacity: 0;
		animation: fadeIn 0.3s ease forwards;
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateX(-20px);
		}
		to {
			opacity: 1;
			transform: translateX(0);
		}
	}

	.nameplate.orange {
		background: linear-gradient(90deg, rgba(255, 150, 39, 0.9) 0%, rgba(189, 122, 36, 0.7) 100%);
		border-left: 4px solid #ff9627;
		box-shadow: 0 4px 12px rgba(255, 150, 39, 0.5);
	}

	.nameplate.blue {
		background: linear-gradient(-90deg, rgba(72, 144, 221, 0.9) 0%, rgba(63, 106, 225, 0.7) 100%);
		border-right: 4px solid #4890dd;
		box-shadow: 0 4px 12px rgba(72, 144, 221, 0.5);
		flex-direction: row-reverse;
	}

	.nameplate.possession {
		transform: scale(1.05);
		filter: brightness(1.2);
	}

	.nameplate.possession::before {
		content: '';
		position: absolute;
		top: 0;
		left: 0;
		right: 0;
		bottom: 0;
		border-radius: 8px;
		animation: pulse 1s ease infinite;
	}

	.nameplate.orange.possession::before {
		box-shadow: 0 0 20px rgba(255, 150, 39, 0.8);
	}

	.nameplate.blue.possession::before {
		box-shadow: 0 0 20px rgba(72, 144, 221, 0.8);
	}

	@keyframes pulse {
		0%,
		100% {
			opacity: 0.5;
		}
		50% {
			opacity: 1;
		}
	}

	.nameplate.stunned {
		opacity: 0.6;
		filter: grayscale(0.5);
	}

	.player_name {
		flex-grow: 1;
		color: white;
		z-index: 1;
	}

	.player_number {
		font-size: 16px;
		color: rgba(255, 255, 255, 0.8);
		margin-left: 10px;
		z-index: 1;
	}

	.nameplate.blue .player_number {
		margin-left: 0;
		margin-right: 10px;
	}

	.possession_indicator {
		width: 12px;
		height: 12px;
		border-radius: 50%;
		background: white;
		margin-left: 10px;
		opacity: 0;
		transition: opacity 0.3s ease;
		z-index: 1;
	}

	.nameplate.blue .possession_indicator {
		margin-left: 0;
		margin-right: 10px;
	}

	.nameplate.possession .possession_indicator {
		opacity: 1;
		animation: possessionBlink 0.8s ease infinite;
	}

	@keyframes possessionBlink {
		0%,
		100% {
			transform: scale(1);
		}
		50% {
			transform: scale(1.3);
		}
	}

	.stunned_indicator {
		position: absolute;
		top: -5px;
		right: -5px;
		background: rgba(255, 0, 0, 0.9);
		color: white;
		padding: 2px 8px;
		border-radius: 12px;
		font-size: 12px;
		font-weight: bold;
		z-index: 2;
	}

	.nameplate.orange .stunned_indicator {
		right: auto;
		left: -5px;
	}
</style>
