var server = 'ws://localhost:5000'; 

const socket = new WebSocket(server + '/ws');

socket.onopen = () =>
{
    console.log('Connecting...');
    $('#msgList').val('WebSocket connection opened.');
};

socket.onmessage = function (evt)
{
    console.log('Received Message: ' + evt.data);
    if (evt.data) {
        var content = $('#msgList').val();
        content = content + '\r\n' + evt.data;

        $('#msgList').val(content);
    }
};

socket.onclose = () =>
{
    console.log('Connection closed.');
};


$('#btnSend').on('click', () => {
    var message = $('#txtMsg').val();
    if (message) {
        // See MessageDTO.cs
        socket.send(JSON.stringify(
        {
            AuthorID: ??,
            Content: msg,
        }));
    }
});

