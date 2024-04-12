const webcamElement = document.getElementById('webcam');


var directions = document.getElementById("directions");
var count = document.getElementById("countdown");
var progressBar = document.getElementById("progress");
var text = "Please prepare for capturing ";
var countdown = 3;
var progress = 0;
var finish = 99;
directions.innerText = "Kindly please move your face closer";







function show_value() {

    var counter = setInterval(function () {


        directions.innerText = text + countdown + " ...";
        text = text + countdown + " ";

        if (countdown > 1) {
            countdown = countdown - 1;


        } else {
            take_snapshot();
            clearInterval(counter);
        }


    }, 2000)

}







const webcam = new Webcam(webcamElement, 'user');


let model = null;
let cameraFrame = null;
let running = false
let timeout = null;

$("#start-capture").click(function () {
    startCaptura();
});


webcam.start()
    .then(result => {
        $("#video-container").show();
        console.log("webcam started");
    })
    .catch(err => {
        console.log(err);
    });



async function stopCaptura() {
    if (running) {
        $("#capture-running").hide();
        $("#start-capture").show();
        $("#info-text").hide();

        running = false;

        if (cameraFrame != null) {
            cancelAnimationFrame(cameraFrame);
        }
    }
}

async function startCaptura() {

    $("#start-capture").hide();
    $('#info-text').text('Loading...');
    $("#info-text").show();


    faceLandmarksDetection.load(

        faceLandmarksDetection.SupportedPackages.mediapipeFacemesh,
        { maxFaces: 1 }

    ).then(mdl => {

        $("#capture-running").show();
        $('#info-text').text('Kindly blink for the camera.');

        model = mdl;
        cameraFrame = detectKeyPoints();

        timeout = setTimeout(() => {
            stopCaptura();
        }, 10000);

        running = true;

    }).catch(err => {

        console.log(err);
        stopCaptura();

    });

}

async function main() {

    await setupFaceLandmarkDetection()
}

async function setupFaceLandmarkDetection() {


    await tf.setBackend('wasm');

}

async function detectaPiscada(keypoints) {
    return true;
}


let blinkCount = 0;
async function detectKeyPoints() {




    const predictions = await model.estimateFaces({
        input: document.querySelector("video"),
        returnTensors: false,
        flipHorizontal: true,
        predictIrises: true
    });

    if (predictions.length > 0) {

        const keypoints = predictions[0].scaledMesh;

        if (detectarPiscada(keypoints)) {

            blinkCount++;  // Increment blink count
            console.log('-----> Blink (' + blinkCount + ' times)');



            if (blinkCount === 1) {
                clearTimeout(timeout);
                stopCaptura();
                // alert('1 blinks detected!');
                progress += 33;
                directions.innerText = "Blink Detected (1/3)";
                progressBar.style.width = progress + "%";
                progressBar.ariaValueNow = progress;
                console.log(progressBar.ariaValueNow);


            }
            if (blinkCount === 2) {
                clearTimeout(timeout);
                stopCaptura();
                //  alert('2 blinks detected!');
                progress += 33;
                directions.innerText = "Blink Detected (2/3)";
                progressBar.style.width = progress + "%";
                progressBar.ariaValueNow = progress;
                console.log(progressBar.ariaValueNow);

            }
            if (blinkCount === 3) {
                clearTimeout(timeout);
                stopCaptura();
                // alert('3 blinks detected! Verified!!');
                progress += 33;
                directions.innerText = "Blink Detected (3/3)";
                progressBar.style.width = progress + "%";
                progressBar.ariaValueNow = progress;
                console.log(progressBar.ariaValueNow);
                //  var co = setInterval(c, 1000);
                show_value();
                progress += 1;
                progressBar.style.width = progress + "%";
                progressBar.ariaValueNow = progress;
                blinkCount = 0;
                return null;  // Stop further execution of detectKeyPoints



            }


            //function c() {
            //    directions.innerText = text + countdown + " ...";
            //    text = text + countdown + " ";
            //    if (countdown > 1) {
            //        countdown = countdown - 1;
            //    } else {
            //        clearInterval(co);
            //    }

            //}







        }

    }

    cameraFrame = requestAnimationFrame(detectKeyPoints);
}

function detectarPiscada(keypoints) {

    leftEye_l = 263
    leftEye_r = 362
    leftEye_t = 386
    leftEye_b = 374

    rightEye_l = 133
    rightEye_r = 33
    rightEye_t = 159
    rightEye_b = 145

    aL = euclidean_dist(keypoints[leftEye_t][0], keypoints[leftEye_t][1], keypoints[leftEye_b][0], keypoints[leftEye_b][1]);
    bL = euclidean_dist(keypoints[leftEye_l][0], keypoints[leftEye_l][1], keypoints[leftEye_r][0], keypoints[leftEye_r][1]);
    earLeft = aL / (2 * bL);

    aR = euclidean_dist(keypoints[rightEye_t][0], keypoints[rightEye_t][1], keypoints[rightEye_b][0], keypoints[rightEye_b][1]);
    bR = euclidean_dist(keypoints[rightEye_l][0], keypoints[rightEye_l][1], keypoints[rightEye_r][0], keypoints[rightEye_r][1]);
    earRight = aR / (2 * bR);

    console.log('-----> ' + earLeft + '\t' + earRight);

    if ((earLeft < 0.1) || (earRight < 0.1)) {
        return true;
    } else {
        return false;
    }

}

function euclidean_dist(x1, y1, x2, y2) {
    return Math.sqrt(Math.pow((x1 - x2), 2) + Math.pow((y1 - y2), 2));
};

main();




