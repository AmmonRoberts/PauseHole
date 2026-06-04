import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig(() => {
	return {
		build: {
			outDir: 'build',
		},
		plugins: [react()],
		server: {
			port: Number.parseInt(process.env.PORT || '5173'),
			host: true, // Listen on all addresses
			strictPort: true, // Fail if port is already in use
		},
	};
});