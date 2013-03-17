function insert(message, user, request) {
    request.execute( {
        success : function() {
            request.respond();
            push.wns.sendToastImageAndText03(message.channel,
             { 
                 image1src : 'https://twimg0-a.akamaihd.net/profile_images/2254646642/IMAG0081.jpg',
                 image1alt : 'tanaka_733', 
                 text1: message.text,
                 text2: message.detail 
             },
             { 
                 success: function(pushResponse) {
                    console.log("Sent push:", pushResponse);
                 }
             });
        } 
    });
}