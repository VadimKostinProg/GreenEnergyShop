async function confirmOrder(orderId) {
    let url = `/Admin/Orders/Confirm?orderId=${orderId}`;

    let response = await fetch(url, { method: "POST" });

    let confirmTile = document.getElementById(`confirmTile{${orderId}}`);
    confirmTile.innerHTML = "<p>Підтверджено</p>";
}