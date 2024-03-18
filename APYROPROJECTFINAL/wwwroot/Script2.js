const imageUpload = document.getElementById('imageUpload')





Promise.all([
        faceapi.nets.faceRecognitionNet.loadFromUri('/models'),
        faceapi.nets.faceLandmark68Net.loadFromUri('/models'),
        faceapi.nets.ssdMobilenetv1.loadFromUri('/models')
]).then(start)




async function start() {
    const labeledFaceDescriptors = await loadLabeledImages();


    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, 0.4);

    imageUpload.addEventListener('change', async () => {
        const container = document.getElementById('modal-container');
        container.innerHTML = '';  // Clear previous content

        const image = await faceapi.bufferToImage(imageUpload.files[0]);
        const canvas = faceapi.createCanvasFromMedia(image);
        const displaySize = { width: image.width, height: image.height };




        // Resize the canvas to match the image size
        canvas.style.width = '100%';
        canvas.style.height = '100%';




        faceapi.matchDimensions(canvas, displaySize);
        const detections = await faceapi.detectAllFaces(image).withFaceLandmarks().withFaceDescriptors();
        const resizedDetections = faceapi.resizeResults(detections, displaySize);
        const results = resizedDetections.map(d => faceMatcher.findBestMatch(d.descriptor));





        results.forEach((result, i) => {
            const box = resizedDetections[i].detection.box;
            const drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() });
            drawBox.draw(canvas);



            console.log(`Recognized Face: ${result.toString()}. Student Name: ${result.label}`);


            matchWithClassCode(result.label);
        });

        // Resize the image

        /*
        image.style.width = 'auto';  // Adjust the width as needed
        image.style.height = 'auto'; // Maintain aspect ratio
        */

        image.style.width = '100%';  // Adjust the width as needed
        image.style.height = '100%'; // Maintain aspect ratio

        container.appendChild(image);
        container.appendChild(canvas);
        showModal();
    });
}





//async function matchWithClassCode(studentName) {
//    try {
//        const data = await $.get('/ClassroomDBs/GetClassroomData');

//        if (data && data.length > 0) {
//            for (const item of data) {
//                if (item.studentName === studentName) {
//                    // Found a match for the student name
//                    const correspondingClassCode = item.classCode;
//                    console.log(`Student Name: ${studentName}, Corresponding Class Code: ${correspondingClassCode}`);
//                    processStudentData(studentName, correspondingClassCode);
//                    return correspondingClassCode; // Return the class code
//                }
//            }
//            // Student name not found
//            console.log(`Student Name '${studentName}' not found in the data.`);
//            return null; // Return null if not found
//        } else {
//            console.log("No data found.");
//            return null; // Return null if no data available
//        }
//    } catch (error) {
//        console.error("An error occurred:", error);
//        return null; // Return null if an error occurs
//    }
//}






async function matchWithClassCode(studentName) {
    try {
        const data = await $.get('/ClassroomDBs/GetClassroomData');

        if (data && data.length > 0) {
            for (const item of data) {
                if (item.studentName === studentName) {
                    const correspondingClassCode = item.classCode;
                    console.log(`Student Name: ${studentName}, Corresponding Class Code: ${correspondingClassCode}`);
                    processStudentData(studentName, correspondingClassCode);
                }
            }
        } else {
            console.log("No data found.");
        }
    } catch (error) {
        console.error("An error occurred:", error);
    }
}



















function processStudentData(studentName, correspondingClassCode)
{

    console.log(`Processing student data for ${studentName} with Class Code: ${correspondingClassCode}`);
    console.log(classId); 

    const data = {
        studentName: studentName,
        classCode: correspondingClassCode,
        classId: classId
    };

    fetch('/ClassroomDBs/ProcessStudentData', {
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





























//async function start() {
//    const labeledFaceDescriptors = await loadLabeledImages();


//    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, 0.4);

//    imageUpload.addEventListener('change', async () =>
//    {
//        const container = document.getElementById('modal-container');
//        container.innerHTML = '';  // Clear previous content

//        const image = await faceapi.bufferToImage(imageUpload.files[0]);
//        const canvas = faceapi.createCanvasFromMedia(image);
//        const displaySize = { width: image.width, height: image.height };




//        // Resize the canvas to match the image size
//        canvas.style.width = '100%';
//        canvas.style.height = '100%';
     



//        faceapi.matchDimensions(canvas, displaySize);
//        const detections = await faceapi.detectAllFaces(image).withFaceLandmarks().withFaceDescriptors();
//        const resizedDetections = faceapi.resizeResults(detections, displaySize);
//        const results = resizedDetections.map(d => faceMatcher.findBestMatch(d.descriptor));





//        results.forEach((result, i) => {
//            const box = resizedDetections[i].detection.box;
//            const drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() });
//            drawBox.draw(canvas);



//            console.log(`Recognized Face: ${result.toString()}. Student Name: ${result.label}`);


//            matchWithClassCode(result.label);
//        });

//        // Resize the image

//        /*
//        image.style.width = 'auto';  // Adjust the width as needed
//        image.style.height = 'auto'; // Maintain aspect ratio
//        */

//        image.style.width = '100%';  // Adjust the width as needed
//        image.style.height = '100%'; // Maintain aspect ratio
   
//        container.appendChild(image);
//        container.appendChild(canvas);
//        showModal();
//    });
//}





//async function matchWithClassCode(studentName) {
//    try {
//        const data = await $.get('/ClassroomDBs/GetClassroomData');

//        if (data && data.length > 0) {
//            for (const item of data) {
//                if (item.studentName === studentName) {
//                    // Found a match for the student name
//                    const correspondingClassCode = item.classCode;
//                    console.log(`Student Name: ${studentName}, Corresponding Class Code: ${correspondingClassCode}`);
//                    processStudentData(studentName, correspondingClassCode);
//                    return correspondingClassCode; // Return the class code
//                }
//            }
//            // Student name not found
//            console.log(`Student Name '${studentName}' not found in the data.`);
//            return null; // Return null if not found
//        } else {
//            console.log("No data found.");
//            return null; // Return null if no data available
//        }
//    } catch (error) {
//        console.error("An error occurred:", error);
//        return null; // Return null if an error occurs
//    }
//}





//function processStudentData(studentName, correspondingClassCode)
//{

//    console.log(`Processing student data for ${studentName} with Class Code: ${correspondingClassCode}`);

//    const data = {
//        studentName: studentName,
//        classCode: correspondingClassCode
//    };

//    fetch('/ClassroomDBs/ProcessStudentData', {
//        method: 'POST',
//        headers: {
//            'Content-Type': 'application/json'
//        },
//        body: JSON.stringify(data)
//    })
//        .then(response => {
//            if (response.ok) {
//                return response.json();
//            }
//            throw new Error('Network response was not ok.');
//        })
//        .then(data => {
//            console.log('Server response:', data);
//            // Handle the server response here
//        })
//        .catch(error => {
//            console.error('There has been a problem with your fetch operation:', error);
//        });

//}















//async function start() {
//    const labeledFaceDescriptors = await loadLabeledImages();


//    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, 0.4);

//    imageUpload.addEventListener('change', async () => {
//        const container = document.getElementById('modal-container');
//        container.innerHTML = '';  // Clear previous content

//        const image = await faceapi.bufferToImage(imageUpload.files[0]);
//        const canvas = faceapi.createCanvasFromMedia(image);
//        const displaySize = { width: image.width, height: image.height };




//        // Resize the canvas to match the image size
//        canvas.style.width = '100%';
//        canvas.style.height = '100%';




//        faceapi.matchDimensions(canvas, displaySize);
//        const detections = await faceapi.detectAllFaces(image).withFaceLandmarks().withFaceDescriptors();
//        const resizedDetections = faceapi.resizeResults(detections, displaySize);
//        const results = resizedDetections.map(d => faceMatcher.findBestMatch(d.descriptor));



//        results.forEach((result, i) => {
//            const box = resizedDetections[i].detection.box;
//            const drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() });
//            drawBox.draw(canvas);
//        });

//        // Resize the image

//        /*
//        image.style.width = 'auto';  // Adjust the width as needed
//        image.style.height = 'auto'; // Maintain aspect ratio
//        */

//        image.style.width = '100%';  // Adjust the width as needed
//        image.style.height = '100%'; // Maintain aspect ratio

//        container.appendChild(image);
//        container.appendChild(canvas);
//        showModal();
//    });
//}





//async function Show() {
//    var data = await $.get('/ClassroomDBs/GetClassroomData');

//    if (data && data.length > 0) {
//        console.log("Received data:", data);
//        data.map(function (item) {
//            const classCode = item.classCode;
//            const studentName = item.studentName;
//            const imageFile = item.imageFile;



//            // Log classCode and studentName
//            console.log("ClassCode: " + classCode);
//            console.log("StudentName: " + studentName);

//            // Log the image source
//            console.log("Image Filename:", imageFile);



//            // Construct the file path based on the 'imageFile'
//            const filePath = `/Images/Verified/${imageFile}`;

//            // Load the image as a blob
//            fetch(filePath)
//                .then((response) => response.blob())
//                .then((blob) => {
//                    // Use the blob data with face-api.js for face recognition
//                    // Here you can use the 'blob' variable with face-api.js

//                    // Log the blob data (this will be a blob object)
//                    //console.log("Blob Data:", blob);

//                    //// You can also convert the blob to a data URL for display
//                    //const imageUrl = URL.createObjectURL(blob);
//                    //console.log("Image Data URL:", imageUrl);


//                    const imgElement = document.createElement('img');

//                    imgElement.src = URL.createObjectURL(blob);

//                    /*console.log("Image Data URL:", imgElement);*/

//                    //const container = document.getElementById('modal-container');
//                    //container.appendChild(imgElement);
//                    //showModal();



//                })
//                .catch((error) => {
//                    console.error('Error loading image:', error);
//                });




//            //const output = `classCode: ${classCode}\nstudentName: ${studentName}\nImageFile: ${imageFile}`;

//            //console.log(output);
//        });
//    } else {
//        console.log("No data found.");
//    }
//}


//Show();












//async function Show() {
//    var data = await $.get('/ClassroomDBs/GetClassroomData');

//    if (data && data.length > 0) {
//        console.log("Received data:", data);
//        data.map(function (item) {
//            const classCode = item.classCode;
//            const studentName = item.studentName;
//            const imageFile = item.imageFile;



//            // Log classCode and studentName
//            console.log("ClassCode: " + classCode);
//            console.log("StudentName: " + studentName);

//            // Log the image source
//            console.log("Image Filename:", imageFile);



//            // Construct the file path based on the 'imageFile'
//            const filePath = `/Images/Verified/${imageFile}`;

//            // Load the image as a blob
//            fetch(filePath)
//                .then((response) => response.blob())
//                .then((blob) => {
//                    // Use the blob data with face-api.js for face recognition
//                    // Here you can use the 'blob' variable with face-api.js

//                    // Log the blob data (this will be a blob object)
//                    console.log("Blob Data:", blob);

//                    // You can also convert the blob to a data URL for display
//                    const imageUrl = URL.createObjectURL(blob);
//                    console.log("Image Data URL:", imageUrl);


//                    const imgElement = document.createElement('img');

//                    imgElement.src = URL.createObjectURL(blob);

//                    console.log("Image Data URL:", imgElement);

//                    // Display the modal



//                })
//                .catch((error) => {
//                    console.error('Error loading image:', error);
//                });




//            //const output = `classCode: ${classCode}\nstudentName: ${studentName}\nImageFile: ${imageFile}`;

//            //console.log(output);
//        });
//    } else {
//        console.log("No data found.");
//    }
//}


//Show();






//async function Show() {
//    var data = await $.get('/ClassroomDBs/GetClassroomData');

//    if (data && data.length > 0) {
//        console.log("Received data:", data);
//        data.map(function (item) {
//            const classCode = item.classCode;
//            const studentName = item.studentName;
//            const imageFile = item.imageFile;

//            const output = `classCode: ${classCode}\nstudentName: ${studentName}\nImageFile: ${imageFile}`;

//            console.log(output);
//        });
//    } else {
//        console.log("No data found.");
//    }
//}


//Show();





function showModal() {
    const modal = document.getElementById('myModal');

    
    modal.style.display = 'block';

    // Center the modal
    modal.style.position = 'fixed';
    modal.style.top = '50%';
    modal.style.left = '50%';
    modal.style.transform = 'translate(-50%, -50%)';

    // Adjust modal height
    modal.style.maxHeight = '90%'; // Adjust the maximum height as needed
    modal.style.overflowY = 'auto'; // Enable vertical scrolling if needed
    
    
}








//async function loadLabeledImages()
//{


//    var data = await $.get('/ClassroomDBs/GetClassroomData');






//    if (data && data.length > 0) {
//        const labeledFaceDescriptors = [];

//        for (const item of data) {
//            const classCode = item.classCode;
//            const studentName = item.studentName;
//            const imageFile = item.imageFile;

//            // Log classCode and studentName


//            //console.group("Item");

//            //console.log("ClassCode: " + classCode);
//            //console.log("StudentName: " + studentName);

//            //// Log the image source
//            //console.log("Image Filename:", imageFile);


//            //console.groupEnd();



//            // Construct the file path based on the 'imageFile'
//            const filePath = `/Images/Verified/${imageFile}`;

//            // Load the image as a blob
//            const blob = await fetch(filePath).then((response) => response.blob());

//            // Create an img element from the blob
//            const imgElement = document.createElement('img');
//            imgElement.src = URL.createObjectURL(blob);


//            const labels = [studentName];
//            const descriptions = [];

//            // Create descriptors from the image
//            const detections = await faceapi.detectSingleFace(imgElement).withFaceLandmarks().withFaceDescriptor();
//            descriptions.push(detections.descriptor);

//            const labeledFaceDescriptor = new faceapi.LabeledFaceDescriptors(studentName, descriptions);
//            labeledFaceDescriptors.push(labeledFaceDescriptor);
//        }

//        return labeledFaceDescriptors;
//    }
//}





async function loadLabeledImages()
{


    var data = await $.get('/ClassroomDBs/GetClassroomData');






    if (data && data.length > 0) {
        const labeledFaceDescriptors = [];

        for (const item of data) {
            const classCode = item.classCode;
            const studentName = item.studentName;
            const imageFile = item.imageFile;

            // Log classCode and studentName


            //console.group("Item");

            //console.log("ClassCode: " + classCode);
            //console.log("StudentName: " + studentName);

            //// Log the image source
            //console.log("Image Filename:", imageFile);


            //console.groupEnd();



            // Construct the file path based on the 'imageFile'
            const filePath = `/Images/Verified/${imageFile}`;

            // Load the image as a blob
            const blob = await fetch(filePath).then((response) => response.blob());

            // Create an img element from the blob
            const imgElement = document.createElement('img');
            imgElement.src = URL.createObjectURL(blob);


            const labels = [studentName];
            const descriptions = [];

            // Create descriptors from the image
            const detections = await faceapi.detectSingleFace(imgElement).withFaceLandmarks().withFaceDescriptor();
            if (detections) {
                descriptions.push(detections.descriptor);
                const labeledFaceDescriptor = new faceapi.LabeledFaceDescriptors(studentName, descriptions);
                labeledFaceDescriptors.push(labeledFaceDescriptor);
            } else {
                console.log('No face detected for', studentName);
            }

           
        }

        return labeledFaceDescriptors;
    }
}

















//async function loadLabeledImages() {
//    var data = await $.get('/ClassroomDBs/GetClassroomData');

//    if (data && data.length > 0) {
//        const labeledFaceDescriptors = [];

//        for (const item of data) {
//            const classCode = item.classCode;
//            const studentName = item.studentName;
//            const imageFile = item.imageFile;

//            // Log classCode and studentName
//            console.log("ClassCode: " + classCode);
//            console.log("StudentName: " + studentName);

//            // Log the image source
//            console.log("Image Filename:", imageFile);

//            // Construct the file path based on the 'imageFile'
//            const filePath = `/Images/Verified/${imageFile}`;

//            // Load the image as a blob
//            const blob = await fetch(filePath).then((response) => response.blob());

//            // Create an img element from the blob
//            const imgElement = document.createElement('img');
//            imgElement.src = URL.createObjectURL(blob);


//            const labels = [studentName];
//            const descriptions = [];

//            // Create descriptors from the image
//            const detections = await faceapi.detectSingleFace(imgElement).withFaceLandmarks().withFaceDescriptor();
//            descriptions.push(detections.descriptor);

//            const labeledFaceDescriptor = new faceapi.LabeledFaceDescriptors(studentName, descriptions);
//            labeledFaceDescriptors.push(labeledFaceDescriptor);
//        }

//        return labeledFaceDescriptors;
//    }
//}












//async function loadLabeledImages()
//{
//    const labels = ['Patrick Etesam']
//    return Promise.all(
//        labels.map(async label => {
//            const descriptions = []
//            for (let i = 1; i <= 2; i++)
//            {

//                 const img = await faceapi.fetchImage(`https://raw.githubusercontent.com/PatrickEtesamTech/FaceImages/main/Labeled_Images/${label}/${i}.jpg`)
//                const detections = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor()
//                descriptions.push(detections.descriptor)

//            }
//            return new faceapi.LabeledFaceDescriptors(label, descriptions)
//        })
//    )
//}











// Call the Show function to start the process




//async function Show() {
//    classroomId
//    $.get('/ClassroomDBs/GetStudentClassroomData', { classroomId: code }, function (data) {
//        console.log(data); // Log the data to the console
//        // Access student names and filenames
//        var studentNames = data.StudentNames;
//        var fileNames = data.FileNames;
//        var code = data.Code;

//        // Use the student names and filenames as needed
//    });

//}













/**
 
function showModal()

{
    const modal = document.getElementById('myModal');
    modal.style.display = 'block';


    // Center the modal
    modal.style.position = 'fixed';
    modal.style.top = '50%';
    modal.style.left = '50%';
    modal.style.transform = 'translate(-50%, -50%)';

    // Adjust modal height
    modal.style.maxHeight = '90%'; // Adjust the maximum height as needed
    modal.style.overflowY = 'auto'; // Enable vertical scrolling if needed

  


    }
    






 */








/*async function start() {
    const container = document.createElement('div')
    container.style.position = 'relative'
    document.body.append(container)
    const labeledFaceDescriptors = await loadLabeledImages()
    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, 0.4)
    let image
    let canvas
    document.body.append('Loaded')
    imageUpload.addEventListener('change', async () => {
        if (image) image.remove()
        if (canvas) canvas.remove()
        image = await faceapi.bufferToImage(imageUpload.files[0])
        container.append(image)
        canvas = faceapi.createCanvasFromMedia(image)
        container.append(canvas)
        const displaySize = { width: image.width, height: image.height }
        faceapi.matchDimensions(canvas, displaySize)
        const detections = await faceapi.detectAllFaces(image).withFaceLandmarks().withFaceDescriptors()
        const resizedDetections = faceapi.resizeResults(detections, displaySize)
        const results = resizedDetections.map(d => faceMatcher.findBestMatch(d.descriptor))
        results.forEach((result, i) => {
            const box = resizedDetections[i].detection.box
            const drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() })
            drawBox.draw(canvas)
        })
    })
}  */







/*
async function loadLabeledImages() {
    const labels = ['Patrick Etesam']
    return Promise.all(
        labels.map(async label => {
            const descriptions = []
            for (let i = 1; i <= 2; i++)
            {
       
               // const img = await faceapi.fetchImage(`https://raw.githubusercontent.com/PatrickEtesamTech/FaceImages/main/Labeled_Images/${label}/${i}.jpg`)
                const detections = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor()
                descriptions.push(detections.descriptor)
     
            }

            return new faceapi.LabeledFaceDescriptors(label, descriptions)
        })
    )
}


*/

/*
 
async function getLabeledFaceDescriptions() {
const label = "Patrick Etesam";
const descriptions = [];

for (let i = 1; i <= 100; i++) {
try {
const img = await faceapi.fetchImage(`./Images/${label}/${i}.png`);
const detections = await faceapi
.detectSingleFace(img)
.withFaceLandmarks()
.withFaceDescriptor();
descriptions.push(new faceapi.LabeledFaceDescriptors(label, [detections.descriptor]));
} catch (error) {
console.error(`Error processing image ${i}:`, error);
}
}

return descriptions;
}

 
*/