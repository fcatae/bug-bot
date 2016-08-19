
open iframe using #DataCue


myIframe.contentWindow.postMessage('hello', '*');
and in the iframe:
    window.onmessage = function(e){
        if (e.data == 'hello') {
            alert('It works!');
        }
    };