function getNumbers() {
    let outRoman = 0;
    let outArabian = "";
    let url = "http://localhost:60830/api/numeral";
    fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, cors, *same-origin
        headers: {
            'Content-Type': 'application/json',
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: JSON.stringify(document.getElementById("input").value), // body data type must match "Content-Type" header
    })
        .then(response => response.json()).then(json => {
            outArabian = json.num; console.log(outArabian); document.getElementById("arabian").innerHTML = outArabian;
            outRoman = json.romanian; console.log(outRoman); document.getElementById("roman").innerHTML = outRoman; console.log(json);
        });
}


