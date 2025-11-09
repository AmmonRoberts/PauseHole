let config = null;

export async function loadConfig() {
	if (!config) {
		const response = await fetch('/config.json');
		config = await response.json();
	}
	return config;
}

export function getConfig() {
	if (!config) {
		throw new Error('Config not loaded. Call loadConfig() first.');
	}
	return config;
}