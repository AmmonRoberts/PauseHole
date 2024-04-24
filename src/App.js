import { useState } from 'react';
import './App.css';
import { Button } from '@mui/material';
import axios from "axios";
import { TextField } from '@mui/material';
import Box from '@mui/material/Box';

function App() {
	const [minutes, setMinutes] = useState(5);

	const handleChange = (event) => {
		setMinutes(event.target.value);
	}

	const pausePiHoles = () => {
		console.log(minutes);
		console.log(process.env);

		axios.get(`http://${process.env.REACT_APP_PIHOLE_0_ADDRESS}/admin/api.php?disable=${minutes * 60}&auth=${process.env.REACT_APP_PIHOLE_0_AUTH}`)
			.then(response => {
				console.log(response);
			})
			.catch(error => {
				console.error(error);
			});

		axios.get(`http://${process.env.REACT_APP_PIHOLE_1_ADDRESS}/admin/api.php?disable=${minutes * 60}&auth=${process.env.REACT_APP_PIHOLE_1_AUTH}`)
			.then(response => {
				console.log(response);
			})
			.catch(error => {
				console.error(error);
			});
	}

	const unPausePiHoles = () => {
		console.log(minutes);

		axios.get(`http://${process.env.REACT_APP_PIHOLE_0_ADDRESS}/admin/api.php?enable&auth=${process.env.REACT_APP_PIHOLE_0_AUTH}`)
			.then(response => {
				console.log(response);
			})
			.catch(error => {
				console.error(error);
			});

		axios.get(`http://${process.env.REACT_APP_PIHOLE_1_ADDRESS}/admin/api.php?enable&auth=${process.env.REACT_APP_PIHOLE_1_AUTH}`)
			.then(response => {
				console.log(response);
			})
			.catch(error => {
				console.error(error);
			});
	}

	return (
		<div className="App">
			<header className="App-header">
				<h1>Disable PiHole</h1>
				<div>
					<TextField label="How long (in minutes)?" variant="outlined" onChange={handleChange} value={minutes} />
				</div>
				<Box sx={{ '& button': { m: 2 } }}>
					<Button className="button" color="error" onClick={pausePiHoles} variant="contained">Disable</Button>
					<Button className="button" color="success" onClick={unPausePiHoles} variant="contained">Enable</Button>
				</Box>

			</header>
		</div>
	);
}

export default App;