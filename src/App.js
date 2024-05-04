import { useState } from 'react';
import './App.css';
import { Button } from '@mui/material';
import axios from "axios";
import { TextField } from '@mui/material';
import Box from '@mui/material/Box';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useEffect } from 'react';
import ReactLoading from 'react-loading';


function App() {
	const [minutes, setMinutes] = useState(5);
	const [pihole0Status, setPihole0Status] = useState();
	const [pihole1Status, setPihole1Status] = useState();
	const [piHole1Loading, setPiHole0Loading] = useState(true);
	const [piHole2Loading, setPiHole1Loading] = useState(true);

	useEffect(() => {
		getStatus();

		let interval = setInterval(async () => {
			getStatus();
		}, 30000);
		return () => {
			clearInterval(interval);
		};
	}, []);

	const handleChange = (event) => {
		setMinutes(event.target.value);
	}

	const pausePiHoles = () => {
		axios.get(`http://${process.env.REACT_APP_PIHOLE_0_ADDRESS}/admin/api.php?disable=${minutes * 60}&auth=${process.env.REACT_APP_PIHOLE_0_AUTH}`)
			.then(response => {
				toast.success(`PiHole at ${process.env.REACT_APP_PIHOLE_0_ADDRESS} disabled for ${minutes} minutes.`, {
					position: "bottom-right",
				})
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error disabling PiHole at ${process.env.REACT_APP_PIHOLE_0_ADDRESS}.`, {
					position: "bottom-right",
				});
			});

		axios.get(`http://${process.env.REACT_APP_PIHOLE_1_ADDRESS}/admin/api.php?disable=${minutes * 60}&auth=${process.env.REACT_APP_PIHOLE_1_AUTH}`)
			.then(response => {
				toast.success(`PiHole at ${process.env.REACT_APP_PIHOLE_1_ADDRESS} disabled for ${minutes} minutes.`, {
					position: "bottom-right",
				})
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error disabling PiHole at ${process.env.REACT_APP_PIHOLE_1_ADDRESS}.`, {
					position: "bottom-right",
				});
			});

		getStatus();
	}

	const unPausePiHoles = () => {
		axios.get(`http://${process.env.REACT_APP_PIHOLE_0_ADDRESS}/admin/api.php?enable&auth=${process.env.REACT_APP_PIHOLE_0_AUTH}`)
			.then(response => {
				toast.success(`PiHole at ${process.env.REACT_APP_PIHOLE_0_ADDRESS} enabled.`, {
					position: "bottom-right",
				});
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error enabling PiHole at ${process.env.REACT_APP_PIHOLE_0_ADDRESS}.`, {
					position: "bottom-right",
				});
			});

		axios.get(`http://${process.env.REACT_APP_PIHOLE_1_ADDRESS}/admin/api.php?enable&auth=${process.env.REACT_APP_PIHOLE_1_AUTH}`)
			.then(response => {
				toast.success(`PiHole at ${process.env.REACT_APP_PIHOLE_1_ADDRESS} enabled.`, {
					position: "bottom-right",
				});
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error enabling PiHole at ${process.env.REACT_APP_PIHOLE_1_ADDRESS}.`, {
					position: "bottom-right",
				});
			});

		getStatus();
	}

	return (
		<div className="App">
			<header className="App-header">
				<h1>PiHole Status</h1>
				<div>
					<h4>PiHole 0:</h4>
					<div>{pihole0Status}{piHole1Loading && <ReactLoading type={'spin'} color={'white'} height={'25%'} width={'25%'} />}</div>
					<h4>PiHole 1:</h4>
					<div>{pihole1Status}{piHole2Loading && <ReactLoading type={'spin'} color={'white'} height={'25%'} width={'25%'} />}</div>
				</div>
				<h1>Disable PiHole</h1>
				<div>
					<TextField className='textField' label="How long (in minutes)?" variant="filled" onChange={handleChange} value={minutes} sx={{
						backgroundColor: '#afb1b4',
						color: 'white',
					}} />
				</div>
				<Box sx={{ '& button': { m: 2 } }}>
					<Button className="button" color="error" onClick={pausePiHoles} variant="contained">Disable</Button>
					<Button className="button" color="success" onClick={unPausePiHoles} variant="contained">Enable</Button>
				</Box>
			</header>
			<ToastContainer />
		</div>
	);

	function getStatus() {
		console.log('Getting status');
		axios.get(`http://${process.env.REACT_APP_PIHOLE_0_ADDRESS}/admin/api.php?summaryRaw&auth=${process.env.REACT_APP_PIHOLE_0_AUTH}`)
			.then(response => {
				setPihole0Status(response.data.status);
				setPiHole0Loading(false);
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error getting status for PiHole at ${process.env.REACT_APP_PIHOLE_0_ADDRESS}.`, {
					position: "bottom-right",
				});
			});

		axios.get(`http://${process.env.REACT_APP_PIHOLE_1_ADDRESS}/admin/api.php?summaryRaw&auth=${process.env.REACT_APP_PIHOLE_1_AUTH}`)
			.then(response => {
				setPihole1Status(response.data.status);
				setPiHole1Loading(false);
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error getting status for PiHole at ${process.env.REACT_APP_PIHOLE_1_ADDRESS}.`, {
					position: "bottom-right",
				});
			});
	}
}

export default App;