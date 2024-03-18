const video = document.getElementById('videoInput')
var media;


document.getElementById("videoInput").addEventListener("change", function () {
    media = URL.createObjectURL(this.files[0]);
    video.style.display = "block";
    video.src = media;
    $("#myModal1").modal("toggle");
    /* video.play();*/
});



console.log("Status is: " + status);


//Promise.all([
//    faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
//    faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
//    faceapi.nets.ssdMobilenetv1.loadFromUri('/models')
//]).then(start)


if (status === "ON") {
    Promise.all([
        faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
        faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
        faceapi.nets.ssdMobilenetv1.loadFromUri('/models')
    ]).then(start);
}
else
{
    // Handle the case when status is not "ON"
    console.log("Status is not ON. Promise not executed.");
}



//async function getData() {
//    try {
//        const data = await $.get('/Home/GetTrackerData');
///*        console.log(data); // Use the data as needed*/

//        for (const item of data)
//        {
//                const classCode = item.classCode;
//                const studentName = item.studentName;
//                const imageFile = item.imageFile;
///*                console.log(data);*/
//                console.log("ClassCode: " + classCode);
//                console.log("StudentName: " + studentName);
//                console.log("Image Filename:", imageFile);
//            }
//    } catch (error) {
//        console.error(error);
//    }
//}

/*getData();*/




//async function getData() {
//    try {
//        const data = await $.get('/Home/GetTrackerData');
//        if (data && data.length > 0) {
//            for (const item of data) {
//                const classCode = item.classCode;
//                const studentName = item.studentName;
//                const imageFile = item.imageFile;
//                console.log(data);
//                console.log("ClassCode: " + classCode);
//                console.log("StudentName: " + studentName);
//                console.log("Image Filename:", imageFile);
//            }
//        }
//    }
//    catch (error)
//    {
//        console.error(error);
//        console.log("Error");
//    }
//}

//getData();













function stopVideo()
{
  
    const modal = document.getElementById('myModal1');
    modal.style.display = 'none';
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
        backdrop.parentNode.removeChild(backdrop);
    }
     
/*     video.pause();*/
}





function start() {
    document.body.append('Models Loaded')






    navigator.getUserMedia(
        { video: {} },
        stream => video.srcObject = stream,
        err => console.error(err)
    )



    /*    video.src = media;*/


    //video.src = '../videos/sample4.mp4'

    /*    video.src = media;*/

    console.log('video added')
  
    recognizeFaces()


}




async function recognizeFaces()
{

    const labeledDescriptors = await loadLabeledImages()
    console.log(labeledDescriptors)
    const faceMatcher = new faceapi.FaceMatcher(labeledDescriptors, 0.4)


    video.addEventListener('play', async () =>
    {
        console.log('Playing')
        stopVideo()
        const canvas = faceapi.createCanvasFromMedia(video)
        document.body.append(canvas)
    

        // FOR MODAL VIDEO RECOGNITION OR CAN ALSO CAM RECOGNITION
        //const canvasContainer = document.getElementById('canvasContainer1');
        //canvasContainer.innerHTML = ''; // Clear the container in case the canvas was already appended
        //canvasContainer.append(canvas);


        const displaySize = { width: video.width, height: video.height }
        faceapi.matchDimensions(canvas, displaySize)



        setInterval(async () => {
            const detections = await faceapi.detectAllFaces(video).withFaceLandmarks().withFaceDescriptors()

            const resizedDetections = faceapi.resizeResults(detections, displaySize)

            canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height)

            const results = resizedDetections.map((d) => {
                return faceMatcher.findBestMatch(d.descriptor)
            })
            results.forEach((result, i) => {
                const box = resizedDetections[i].detection.box
                const drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() })
                drawBox.draw(canvas)

                console.log(`Recognized Face: ${result.toString()}. Student Name: ${result.label}`);

                matchWithClassCode(result.label);

            })
        }, 5000)



    })
}





async function matchWithClassCode(studentName) {
    try {
        const data = await $.get('/Home/GetTrackerData');

        if (data && data.length > 0) {
            for (const item of data) {
                if (item.studentName === studentName) {
                    const correspondingClassCode = item.classCode;
                    console.log(`Student Name: ${studentName}, Corresponding Class Code: ${correspondingClassCode}`);
                    processStudentData1(studentName, correspondingClassCode);
                }
            }
        } else {
            console.log("No data found.");
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }
}






function processStudentData1(studentName, correspondingClassCode) {

    console.log(`Processing student data for ${studentName} with Class Code: ${correspondingClassCode}`);
    console.log(classId1);

    const data = {
        studentName: studentName,
        classCode: correspondingClassCode,
        classId: classId1
    };




    fetch('/Home/ProcessTracker', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error('Network response was not ok.');
        })
        .then(data => {
            console.log('Server response:', data);
            // Handle the server response here
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
   
}



















async function loadLabeledImages() {
  

         var data = await $.get('/Home/GetTrackerData');

         const labeledFaceDescriptors = [];
         const totalItems = data.length;
         let itemsProcessed = 0;


    /*     showLoadingModal5();*/


        for (const item of data)
        {
                const classCode = item.classCode;
                const studentName = item.studentName;
                const imageFile = item.imageFile;

                //console.group("Item");
                //console.log("ClassCode: " + classCode);
                //console.log("StudentName: " + studentName);
                //console.log("Image Filename:", imageFile);
                //console.groupEnd();



                itemsProcessed++;
                const progress1 = Math.round((itemsProcessed / totalItems) * 100);
                updateProgressBar1(progress1);



                const filePath = `/Images/Verified/${imageFile}`;
                const blob = await fetch(filePath).then((response) => response.blob());
                const imgElement = document.createElement('img');
                imgElement.src = URL.createObjectURL(blob);


                const labels = [studentName];
                const descriptions = [];

                const detections = await faceapi.detectSingleFace(imgElement).withFaceLandmarks().withFaceDescriptor()

              if (detections)
              {
                descriptions.push(detections.descriptor);
                const labeledFaceDescriptor = new faceapi.LabeledFaceDescriptors(studentName, descriptions);
                labeledFaceDescriptors.push(labeledFaceDescriptor);
              }
               else
              {
                console.log('You have not yet registered your face for verification', studentName);
                displayNoFaceModal5(studentName);
               }
               


         }


    //hideLoadingModal5();
    return labeledFaceDescriptors;
  
}



function updateProgressBar1(progress1)
{
    // Update your progress bar here
    console.log("Progress: " + progress1 + "%");
}

function displayNoFaceModal5(studentName)
{
    $('.studentName5').text(studentName);
    $('#modal10').modal('show');
}
























//async function loadLabeledImages() {


//    var data = await $.get('/Home/GetTrackerData');

//    const labeledFaceDescriptors = [];
//    for (const item of data) {
//        const classCode = item.classCode;
//        const studentName = item.studentName;
//        const imageFile = item.imageFile;
//        console.log("ClassCode: " + classCode);
//        console.log("StudentName: " + studentName);
//        console.log("Image Filename:", imageFile);

//        const filePath = `/Images/Verified/${imageFile}`;
//        const blob = await fetch(filePath).then((response) => response.blob());
//        const imgElement = document.createElement('img');
//        imgElement.src = URL.createObjectURL(blob);


//        const labels = [studentName];
//        const descriptions = [];

//        const detections = await faceapi.detectSingleFace(imgElement).withFaceLandmarks().withFaceDescriptor()

//        descriptions.push(detections.descriptor);

//        const labeledFaceDescriptor = new faceapi.LabeledFaceDescriptors(studentName, descriptions);
//        labeledFaceDescriptors.push(labeledFaceDescriptor);


//    }

//    return labeledFaceDescriptors;

//}







//function loadLabeledImages() {
//    //const labels = ['Black Widow', 'Captain America', 'Hawkeye' , 'Jim Rhodes', 'Tony Stark', 'Thor', 'Captain Marvel']
//    const labels = ['Patrick Etesam']
//    return Promise.all(
//        labels.map(async (label) => {
//            const descriptions = []
//            for (let i = 1; i <= 2; i++) {
//                const img = await faceapi.fetchImage(`/labeled_images/${label}/${i}.jpg`)
//                const detections = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor()
//                console.log(label + i + JSON.stringify(detections))
//                descriptions.push(detections.descriptor)
//            }
//            document.body.append(label + ' Faces Loaded | ')
//            return new faceapi.LabeledFaceDescriptors(label, descriptions)
//        })
//    )
//}

