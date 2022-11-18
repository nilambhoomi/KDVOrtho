'use strict';

var ieId = 0, fuId=0;
let log = console.log.bind(console),
    id = val => document.getElementById(val),
    ul = id('ul'),
    gUMbtn = id('gUMbtn'),
    start = id('start'),
    stop = id('stop'),
    stream,
    recorder,
    counter = 1,
    chunks,
    media,
    type;


let mv = id('mediaVideo'),
    mediaOptions = {
        video: {
            tag: 'video',
            type: 'video/webm',
            ext: '.mp4',
            gUM: { video: true, audio: true }
        },
        audio: {
            tag: 'audio',
            type: 'audio/mp3',//'audio/ogg',
            ext: '.mp3',
            gUM: { audio: true }
        }
    };
media = mediaOptions.audio;
navigator.mediaDevices.getUserMedia(media.gUM).then(_stream => {
    stream = _stream;
    id('gUMArea').style.display = 'none';
    id('btns').style.display = 'inherit';
    start.removeAttribute('disabled');
    recorder = new MediaRecorder(stream);
    recorder.ondataavailable = e => {
        chunks.push(e.data);
        if (recorder.state == 'inactive') makeLink();
    };
    log('got media successfully');
}).catch(log);


function startRecording(startId, stopId) {
    document.getElementById(startId).disabled = true;
    document.getElementById(stopId).removeAttribute('disabled');
    chunks = [];
    recorder.start();
}

function stopRecording(startId, stopId, vtype, vieId, vfuId) {
  
    type= vtype;
    ieId = vieId;
    fuId = vfuId;
    document.getElementById(stopId).disabled = true;
    recorder.stop();
    document.getElementById(startId).removeAttribute('disabled');
}


//start.onclick = e => {
//    start.disabled = true;
//    stop.removeAttribute('disabled');
//    chunks = [];
//    recorder.start();
//}


//stop.onclick = e => {
//    stop.disabled = true;
//    recorder.stop();  
//    start.removeAttribute('disabled');
//}



function makeLink() {
  
    let blob = new Blob(chunks, { type: media.type })
        , url = URL.createObjectURL(blob)
        , li = document.createElement('li')
        , mt = document.createElement(media.tag)
        , hf = document.createElement('a')
        ;
    mt.controls = true;
    mt.src = url;
    hf.href = url;
    hf.download = `${counter++}${media.ext}`;
    hf.innerHTML = `donwload ${hf.download}`;
    li.appendChild(mt);
    li.appendChild(hf);
    ul.appendChild(li);

    var formData = new FormData();
    formData.append("upload", blob);
    $.ajax({
        url: "FileHandler.ashx?method=savefiletodirectory&fileName=test&type=" + type + "&IEID=" + ieId + "&FUID=" + fuId,
        type: 'post',
        data: formData,
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        contentType: false,
        processData: false,
        //cache: false,
        success: function (data) {
            console.log(data);
            location.reload(true);
        },
        error: function (jqXHR, error, errorThrown) {
            console.log(error);
        }
    });
}