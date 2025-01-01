import { Tooltip } from 'react-tooltip'
import InfoIcon from '@mui/icons-material/Info';;

function Status(piHoleStatus) {
	let { address, status, errorMessage } = piHoleStatus;

	return (
		<>
			<h4>{address}</h4>
			<div>{status} {errorMessage && <>
				<a className="my-anchor-element"><InfoIcon /></a>
				<Tooltip anchorSelect=".my-anchor-element" place="top">
					<div>{errorMessage}</div>
				</Tooltip>
			</>}</div>


		</>
	);
}

export default Status;