<script lang="ts">
	import { SparkWebsocket } from '$lib/js/spark_websocket.js';
	import Header from '$lib/components/Header.svelte';
	import 'bulma';

	import { onDestroy } from 'svelte';
	import { httpPostAsync } from '$lib/js/util.js';

	let sw = new SparkWebsocket();
	let config = {
		num_casters: 2,
		caster1_name: '',
		caster1_logo: '',
		caster2_name: '',
		caster2_logo: '',
		caster3_name: '',
		caster3_logo: '',
		caster4_name: '',
		caster4_logo: ''
	};
	let isLoading = true;
	let dirty = false;

	sw.subscribe('overlay_config', (data) => {
		if (!dirty && data && data['caster_prefs']) {
			config = {
				num_casters: data['caster_prefs']['num_casters'] || 2,
				caster1_name: data['caster_prefs']['caster1_name'] || '',
				caster1_logo: data['caster_prefs']['caster1_logo'] || '',
				caster2_name: data['caster_prefs']['caster2_name'] || '',
				caster2_logo: data['caster_prefs']['caster2_logo'] || '',
				caster3_name: data['caster_prefs']['caster3_name'] || '',
				caster3_logo: data['caster_prefs']['caster3_logo'] || '',
				caster4_name: data['caster_prefs']['caster4_name'] || '',
				caster4_logo: data['caster_prefs']['caster4_logo'] || ''
			};
		}
		isLoading = false;
	});

	onDestroy(() => sw.close());

	function saveCasters() {
		isLoading = true;

		// Send via WebSocket
		if (sw.ws.readyState === WebSocket.OPEN) {
			const request = 'set_overlay_config:' + JSON.stringify(config);
			sw.ws.send(request);
			dirty = false;

			setTimeout(() => {
				isLoading = false;
			}, 500);
		} else {
			console.error('WebSocket not open!');
			isLoading = false;
		}
	}

	function valueChanged() {
		dirty = true;
	}
</script>

<svelte:head>
	<title>Caster Setup</title>
</svelte:head>

<Header title="Caster Setup" subtitle="Configure caster names and logos for overlays." />

<div
	class="content"
	style="max-width: 60em; margin: auto; position:relative; overflow-y: auto; max-height: calc(100vh - 200px);"
>
	<nav class="breadcrumb" aria-label="breadcrumbs" style="position: absolute;top: -8em;">
		<ul>
			<li><a href="/">Home</a></li>
			<li><a href="/overlays">Overlays</a></li>
			<li class="is-active"><a href="#" aria-current="page">Caster Setup</a></li>
		</ul>
	</nav>

	<div class="box" style="position: relative; top: -2em;font-size: 1.5em; margin-bottom: 2em;">
		<div class="field">
			<label class="label">Number of Casters</label>
			<div class="control">
				<div class="select">
					<select bind:value={config.num_casters} on:change={valueChanged}>
						<option value={1}>1</option>
						<option value={2}>2</option>
						<option value={3}>3</option>
						<option value={4}>4</option>
					</select>
				</div>
			</div>
		</div>

		<hr />

		{#each Array(config.num_casters) as _, i}
			<div class="box" style="background: rgba(0,0,0,0.05);">
				<h3 class="title is-4">Caster {i + 1}</h3>

				<div class="field">
					<label class="label">Name</label>
					<div class="control">
						<input
							class="input"
							type="text"
							placeholder="Caster Name"
							bind:value={config[`caster${i + 1}_name`]}
							on:input={valueChanged}
						/>
					</div>
				</div>

				<div class="field">
					<label class="label">Logo URL</label>
					<div class="control">
						<input
							class="input"
							type="text"
							placeholder="https://example.com/logo.png"
							bind:value={config[`caster${i + 1}_logo`]}
							on:input={valueChanged}
						/>
					</div>
				</div>

				{#if config[`caster${i + 1}_logo`]}
					<div class="field">
						<label class="label">Preview</label>
						<figure class="image is-128x128">
							<img
								src={config[`caster${i + 1}_logo`]}
								alt="Caster {i + 1} logo"
								style="border-radius: 50%; object-fit: cover;"
							/>
						</figure>
					</div>
				{/if}
			</div>
		{/each}

		<div class="field" style="margin-top: 2em;">
			<div class="control">
				<button
					class="button is-primary is-large is-fullwidth"
					on:click={saveCasters}
					disabled={isLoading}
				>
					{isLoading ? 'Saving...' : 'Save Caster Configuration'}
				</button>
			</div>
		</div>

		{#if dirty}
			<div class="notification is-warning" style="margin-top: 1em;">You have unsaved changes!</div>
		{/if}
	</div>
</div>

<style>
	.label {
		font-weight: bold;
		margin-bottom: 0.5em;
	}
</style>
