function Status(piHoleStatus) {
	let { address, status } = piHoleStatus;

	return (
		<>
			<h4>{address}</h4>
			<div>{status}</div>
		</>
	);
}

export default Status;