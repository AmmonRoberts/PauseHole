import { Tooltip } from 'react-tooltip';
import { Info } from '@mui/icons-material';
function Status(piHoleStatus) {
	let { address, status, errorMessage } = piHoleStatus;

	let shortAddress = address.replace(/^https?:\/\//, '');
	let normalizedAddress = shortAddress.replace(/[^a-zA-Z0-9]/g, '_');

	return (
		<>
			<h4>{shortAddress}</h4>
			<div>{status}
				{errorMessage &&
					<>
						<div className={`my-anchor-element-${normalizedAddress}`}><Info color="error" /></div>
						<Tooltip anchorSelect={`.my-anchor-element-${normalizedAddress}`} place="top">
							<div>{errorMessage}</div>
						</Tooltip>
					</>
				}
			</div>
		</>
	);
}

export default Status;