// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let _path_url = '';
window.location.href.indexOf("localhost") > -1 ? _path_url = `${window.location.origin}/api/` :
    _path_url = `${window.location.origin}/EBookkeepingAuth/api/`;



const step1 = document.querySelector('#step1');
const step2 = document.querySelector('#step2');
const step3 = document.querySelector('#step3');
//const step4 = document.querySelector('#step4');
const verificationBtn = document.querySelector("#verificationBtn");
const ghanaCardConfirmView = document.querySelector("#ghanaCardConfirmView");
const ghanaCardVerificationView = document.querySelector("#ghanaCardVerificationView");
const ghanaCardNumber = document.querySelector("#ghanaCardNumber");
const ghanarequired = document.querySelector("#ghanarequired");
const dobVerificationView = document.querySelector("#dobVerificationView");
const goToDobViewBtn = document.querySelector("#goToDobViewBtn");
const dob = document.querySelector("#dob");
//const backToGhanaCardViewBtn = document.querySelector("#backToGhanaCardViewBtn");
const fullName = document.querySelector("#fullName");
const firstName = document.querySelector("#firstName");
const lastName = document.querySelector("#lastName");
const dobInput = document.querySelector("#dobInput");
const progressBar = document.querySelector("#progressBar");
const verificationSteps = document.querySelector("#verificationSteps");
const phoneNumber = document.querySelector("#phoneNumber");
const password = document.querySelector("#password");
const passwordConfirm = document.querySelector("#passwordConfirm");
const tinGHCard = document.querySelector("#tinGHCard");
const bussinessTinOrGhCard = document.querySelector("#bussinessTinOrGhCard");
let cardInfo,verificationCount =1;




function intializeProgress(type, value, end, bg = "bg-warning") {
    if (progressBar) {
        progressBar.style.width = value;
        progressBar.classList.remove("bg-warning")
        progressBar.classList.remove("bg-success")
        progressBar.classList.add(bg)
        verificationSteps.textContent = `${type} ${verificationCount}/5`;
    }
   
}
intializeProgress('Verification', "0%",2);


goToDobViewBtn.addEventListener("click", () => {
    ghanarequired.textContent = "";
    if (!ghanaCardNumber.value) {
        ghanarequired.textContent = 'The Ghana Card # field is required.'
        return
    }

   
      verfiyGhanaCard()
})

ghanaCardNumber.addEventListener('input', () => {


    if (ghanaCardNumber.value.length === 15) {
        intializeProgress('Verification',"10%",2);
    }

    
})

//phoneNumber.addEventListener('input', () => {
//    if (phoneNumber.value.length > 0) {

//        if (password.value.length === 0 && passwordConfirm.value.length ===0) {

//            intializeProgress('Step', "43%", 2, "bg-warning");
//        }
//        else if (password.value.length > 0 && passwordConfirm.value.length === 0) {

//            intializeProgress('Step', "46%", 2, "bg-warning");
//        }
//        else if (password.value.length > 0 && passwordConfirm.value.length > 0) {

//            intializeProgress('Step', "50%", 2, "bg-warning");
//        }

       
//    }
   
//})


//password.addEventListener('input', () => {
//    if (password.value.length > 0) {

//        if (phoneNumber.value.length === 0 && passwordConfirm.value.length === 0) {

//            intializeProgress('Step', "43%", 2, "bg-warning");
//        }
//        else if (phoneNumber.value.length > 0 && passwordConfirm.value.length === 0) {

//            intializeProgress('Step', "46%", 2, "bg-warning");
//        }
//        else if (phoneNumber.value.length > 0 && passwordConfirm.value.length > 0) {

//            intializeProgress('Step', "50%", 2, "bg-warning");
//        }


//    }

//})

//passwordConfirm.addEventListener('input', () => {
//    if (passwordConfirm.value.length > 0) {

//        if (phoneNumber.value.length === 0 && phoneNumber.value.length === 0) {

//            intializeProgress('Step', "43%", 2, "bg-warning");
//        }
//        else if (phoneNumber.value.length > 0 && phoneNumber.value.length === 0) {

//            intializeProgress('Step', "46%", 2, "bg-warning");
//        }
//        else if (phoneNumber.value.length > 0 && phoneNumber.value.length > 0) {

//            intializeProgress('Step', "50%", 2, "bg-warning");
//        }


//    }

//})








//backToGhanaCardViewBtn.addEventListener('click', () => {
//    dobrequired.textContent = "";
//    ghanarequired.textContent = "";
//    dobVerificationView.hidden = true;
//    ghanaCardVerificationView.hidden = false;
//})

verificationBtn.addEventListener('click', () => {
    dobrequired.textContent = "";
    if (!dob.value) {
        dobrequired.textContent = 'This field is required'
        return
    }

    $('.lds-ellipsis').fadeIn(); // will first fade out the loading animation
    $('.preloader').delay(333).fadeIn('slow'); // will fade out the white DIV that covers the website.
    $('body').delay(333);

    setTimeout(() => { verifyDOB()},1000)
    

  
})

function removeExtraSpaces(string) {
    return string.replace(/\s+/g, '').trim();
}


function verifyDOB() {
  
    //
    //new Date(dob.value).getTime() !== new Date().getTime()
    $('.lds-ellipsis').fadeOut(); // will first fade out the loading animation
    $('.preloader').delay(333).fadeOut('slow'); // will fade out the white DIV that covers the website.
    $('body').delay(333);

    const mothersFullName = dob.value.toLowerCase().split(' ')

    let name0 = mothersFullName[0]
    let name1 = mothersFullName[1]
    let name2 = mothersFullName[2]
    let name3 = mothersFullName[3]

    const namefromCard = removeExtraSpaces(cardInfo.mother_maiden_name.toLowerCase())
    console.log(namefromCard)

    if (
        (namefromCard === `${name0}${name1}${name2}`) ||
        (namefromCard === `${name0}${name2}`) ||
        (namefromCard === `${name0}${name1}`) ||
        (namefromCard === `${name1}${name2}`) ||
        (namefromCard === `${name2}${name1}`)
    ) {
        verificationCount = 3

        intializeProgress('Registration', "60%", 3, "bg-info");

        dobVerificationView.hidden = true;
        ghanaCardConfirmView.hidden = false;
        ghanaCardVerificationView.hidden = true;

        fullName.value = `${cardInfo.forenames} ${cardInfo.surname}`;
        lastName.value = cardInfo.surname;
        firstName.value = cardInfo.forenames;

        setTimeout(() => {
            $('.lds-ellipsis').fadeOut(); // will first fade out the loading animation
            $('.preloader').delay(333).fadeOut('slow'); // will fade out the white DIV that covers the website.
            $('body').delay(333);
        }, 1000)
    } else {

        dobrequired.textContent = 'Sorry, your mother maiden name is incorrect.';
        setTimeout(() => {
            $('.lds-ellipsis').fadeOut(); // will first fade out the loading animation
            $('.preloader').delay(333).fadeOut('slow'); // will fade out the white DIV that covers the website.
            $('body').delay(333);
        }, 1000)
    }
   

}







$("#fistStepBtn").click(() => {
    disPlayStepTwo(); 
    verificationCount = 4
    intializeProgress('Registration', "80%", 2, "bg-info");
   
})

$("#backStepBtn").click(() => {
    disPlayStepOne()
    verificationCount = 3
    intializeProgress('Registration', "60%", 2, "bg-info");
   
})

$("#secStepBtn").click(() => {
    disPlayStepThree()
    verificationCount = 5
    intializeProgress('Registration', "100%", 2, "bg-success");
    
})

$("#backStepBtn2").click(() => {
    disPlayStepTwo()
    verificationCount = 4
    intializeProgress('Registration', "80%", 2, "bg-info");
   
})

//$("#backStepBtn3").click(() => {
//    disPlayStepThree()
//})

$("#thirdStepBtn").click(() => {
    displayStepFour()
    intializeProgress('Registration', "100%", 2, "bg-success");
})





function disPlayStepOne() {
    step2.setAttribute("hidden", "hidden");
    step2.classList.remove('animate__slideInLeft');
    $("#step2").fadeOut("fast")

    step1.removeAttribute("hidden");
    step1.classList.remove('animate__slideOutRight');
    step1.classList.add('animate__slideInLeft');

}

function disPlayStepTwo() {

    step1.setAttribute("hidden", "hidden");
    step3.setAttribute("hidden", "hidden");
    step1.classList.add('animate__slideOutRight');
    step2.removeAttribute("hidden")
    step2.classList.add('animate__slideInLeft');

}

function disPlayStepThree() {
    step2.setAttribute("hidden", "hidden");
    //step4.setAttribute("hidden", "hidden");
    //step4.classList.remove('animate__slideInLeft');
    step2.classList.remove('animate__slideInLeft');
    $("#step2").fadeOut("fast")

    step3.removeAttribute("hidden");
    step3.classList.remove('animate__slideOutRight');
    step3.classList.add('animate__slideInLeft');

}

function displayStepFour() {

    step3.setAttribute("hidden", "hidden");
    
    step3.classList.remove('animate__slideInLeft');
    $("#step3").fadeOut("fast")

    //step4.removeAttribute("hidden");
    //step4.classList.remove('animate__slideOutRight');
    //step4.classList.add('animate__slideInLeft');
}

const getapiJson = (url, error = "Sorry, something went wrong!") => {
    $("#global-loader").fadeIn("slow");

    return fetch(url).then(res => {
        if (!res.ok)
            throw new Error(`${error} (${res.status})`)
        return res.json()
    })

}


async function verfiyGhanaCard() {

    $('.lds-ellipsis').fadeIn(); // will first fade out the loading animation
    $('.preloader').delay(333).fadeIn('slow'); // will fade out the white DIV that covers the website.
    $('body').delay(333);

    try {
        let result = await getapiJson(`${_path_url}GhanaCard/GhanaCard/${ghanaCardNumber.value}`)
       
        if (!result) {
            $('.preloader').delay(333).fadeOut('slow');
            $('.lds-ellipsis').fadeOut();
            $('body').delay(333);
            ghanarequired.textContent = 'Sorry, your card number is not valid.'
            throw "In valid card";  
        }

        ghanarequired.textContent = '';

        cardInfo = JSON.parse(result).guinData[0]

        getBodVerificationView()

        intializeProgress('Step', "20%", 2);
        

    } catch (e) {
        console.log(e)
    }

}

let getBodVerificationView = () => {

    //dobVerificationView.hidden = false;

    //ghanaCardConfirmView.hidden = false;

   // ghanaCardVerificationView.hidden = true;
   
    dobInput.hidden = false;
    verificationBtn.hidden = false;
    goToDobViewBtn.hidden = true;
    $('.preloader').delay(333).fadeOut('slow');

    verificationCount = 2

}

let setValue = "";
tinGHCard.addEventListener('input', () => {
    tinGHCard.style.border = "1px solid red";
    if (tinGHCard.value.length === 11) {


        validateTinNumber(tinGHCard.value)
        return
    }
    else if (tinGHCard.value.length === 15) {

        validateGhCardNumber(tinGHCard.value)
    }
    else {
        bussinessTinOrGhCard.textContent = "";
    }
})

async function validateTinNumber(tinNumber) {
    try {

        let result = await getapiJson(`${_path_url}GhanaCard/ValidateTin/${tinNumber}`)
        let resObj = JSON.parse(result)
        if (!resObj.length > 0) {
            bussinessTinOrGhCard.textContent = "Your TIN number is not valid";
             tinGHCard.style.border = "1px solid red";
          
            return;
        }
        tinGHCard.style.border = "1px solid #dae1e3"
        bussinessTinOrGhCard.textContent = "";
        tinGHCard.style.border = "1px solid red";

    } catch (e) {

    }

}

async function validateGhCardNumber(cardNumber) {
    try {
        let result = await getapiJson(`${_path_url}GhanaCard/GhanaCard/${cardNumber}`)

        if (result ==="") {
            bussinessTinOrGhCard.textContent = "Your Ghana card number is not valid";
            tinGHCard.style.border = "1px solid red";

            return;
        }
        bussinessTinOrGhCard.textContent =""

    } catch (e) {

    }
}




