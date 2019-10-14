function getNumbers() {
    let outRoman = 0;
    let outArabian = "";
    let url = "http://91.240.86.250:9092/api/numeral";
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
            outArabian = json.num; console.log(outArabian);
            outRoman = json.romanian;
            errorCode = json.errorCode;
            errorWord = json.errorWord;
            sentence = json.fullSentence;
			ranks = json.ranks
            if (errorCode == 0) {
                document.getElementById("arabian").innerHTML = (outArabian === 0 ? "" : outArabian);
                document.getElementById("roman").innerHTML = outRoman;
            }
            else {
                let errorMessage = getErrorMessage(errorCode, errorWord, sentence, ranks);
                document.getElementById("arabian").innerHTML = errorMessage;
                document.getElementById("roman").innerHTML = "";
            }
            console.log(errorCode);
            console.log(outRoman);
            console.log(json);
        });
}

function getErrorMessage(errorCode = 0, errorWord = "", sentence = "", ranks = []) {
    if (errorCode > 1000 && errorCode < 2000) {
        let pos = Math.floor(errorCode % 1200 / 10) + 1;
        let rank = errorCode % 10;
        let message = "";
        console.log(pos);
        console.log(rank);

        switch (rank) {

            case 1:
                message = ", единицы";
                break;
            case 2:
                message = ", десятки";
                break;
            case 3:
                message = ", два числа формата сотен";
                break;
            case 4:
                message = ", число формата 10-19";
                break;
            default:
                message = "Error";
                break;
        }
		let postmessage = "";
		if(ranks[3] == true)
		{
			if(rank == 4)
			{
				message = ", два числа формата 10-19";
			}
			else
			{
				postmessage = "не может стоять после числа формата 10-19"
			}
		}
		
		else if(ranks[0] == true)
		{
			if(rank == 1)
			{
				message = ", два числа единичного формата";
			}
			else
			{
				postmessage = "не может стоять после единиц"
			}
		}
		else if(ranks[1] == true)
		{
			if(rank == 2)
			{
				message = ", два числа формата десятков";
			}
			else
			{
				postmessage = "не может стоять после десятков"
			}
		}
		if(rank == 3)
		{
			postmessage = "";
			return `Слово \"${errorWord}\" на позиции ${pos} неправильное${message} ${postmessage}!`;
		}
		
        return `Слово \"${errorWord}\" на позиции ${pos} не может стоять после \"${sentence}\"${message} ${postmessage}!`;
    }

    if (errorCode > 219 && errorCode < 230) {
        let pos = Math.floor(errorCode % 220) + 1;
        return `${pos}-ое слово \"${errorWord}\" неправильное!`;
    }

    if (errorCode > 29 && errorCode < 40) {
        let pos = Math.floor(errorCode % 30) + 1;
        return `${pos}-ое слово \"${errorWord}\" неправильное!`;
    }

    switch (errorCode) {

        case 11:
            return `\"${errorWord}\" не может стоять перед hundred!`;
            break;
        case 21:
            return `Слишком большое число слов дано, уберите слово \"${errorWord}\"!`;
            break;
        case 23:
            return `Неправильное слово \"${errorWord}\" на 2-ой позиции, возможно имелось ввиду hundred!`;
            break;
        case 40:
            return `Требуется слово перед hundred!`;
            break;
        default:
            return `Неизвестная ошибка!`;
            break;
    }
}

class ChessLine {
    constructor() {
        this.line1 = "";
        this.line2 = "";
    }
}

function ShuffleLine() {
    let data = new ChessLine();
    data.line1 = document.getElementById("input1").value;
    data.line2 = document.getElementById("input2").value;
    let url = "http://91.240.86.250:9092/api/shuffle";

    fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, cors, *same-origin
        headers: {
            'Content-Type': 'application/json',
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: JSON.stringify(data), // body data type must match "Content-Type" header
    })
        .then(response => response.json()).then(json => {
            result = json.result; console.log(result);
            document.getElementById("output").innerHTML = result;
            console.log(json);
        });
}


