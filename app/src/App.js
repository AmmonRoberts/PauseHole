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
import Status from './Components/Status.js';

function App() {
	const [minutes, setMinutes] = useState(5);
	const [piHoleStatuses, setPiholeStatuses] = useState([]);
	const [piHolesLoading, setPiHolesLoading] = useState(true);

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

	const pausePiHoles = async () => {
		setPiHolesLoading(true);

		// Hard-coding the URL for now. I don't want to deal with config in the frontend.
		await axios.post(`${process.env.REACT_APP_BACKEND_ADDRESS}/pause?minutes=${minutes}`)
			.then(response => {
				setPiholeStatuses(response.data);
				setPiHolesLoading(false);

				toast.success(`PiHoles disabled for ${minutes} minutes.`, {
					position: "bottom-right",
				})
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error disabling PiHoles.`, {
					position: "bottom-right",
				});
			});
	}

	const unPausePiHoles = async () => {
		setPiHolesLoading(true);

		// Hard-coding the URL for now. I don't want to deal with config in the frontend.
		await axios.post(`${process.env.REACT_APP_BACKEND_ADDRESS}/unpause`)
			.then(response => {
				setPiholeStatuses(response.data);
				setPiHolesLoading(false);

				toast.success(`PiHoles enabled.`, {
					position: "bottom-right",
				})
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error enabling PiHoles.`, {
					position: "bottom-right",
				});
			});
	}

	async function getStatus() {
		setPiHolesLoading(true);

		// Hard-coding the URL for now. I don't want to deal with config in the frontend.
		await axios.get(`${process.env.REACT_APP_BACKEND_ADDRESS}/status`)
			.then(response => {
				setPiholeStatuses(response.data);
				setPiHolesLoading(false);
			})
			.catch(error => {
				console.error(error);
				toast.error(`Error getting status for PiHoles.`, {
					position: "bottom-right",
				});
			});
	}

	return (
		<div className="App">
			<header className="App-header">
				<h1>PiHole Status</h1>
				<div>
					{piHolesLoading && <ReactLoading type={'spin'} color={'white'} height={'25%'} width={'25%'} />}
					{!piHolesLoading && piHoleStatuses.map((status, index) => {
						let { address, status: statusText, errorMessage } = status;

						return (<Status key={index} address={address} status={statusText} errorMessage={errorMessage} />)
					})}
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
}

export default App;