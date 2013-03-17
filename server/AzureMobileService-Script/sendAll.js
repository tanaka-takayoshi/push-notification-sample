function sendAll() {
    console.log('Start Send All Messages Job');
    var sql = "SELECT id, text, detail, channel FROM DetailMessage ";

    mssql.query(sql, {
        success: function(results) {
            if (results.length > 0) {
                for (var i = 0; i < results.length; i++) {
                    var message = results[i];
                    push.wns.sendToastImageAndText04(message.channel,
                     { 
                         image1src : 'https://twimg0-a.akamaihd.net/profile_images/2254646642/IMAG0081.jpg',
                         image1alt : 'tanaka_733', 
                         text1: message.text,
                         text2: 'バッチによる実行',
                         text3: message.detail 
                     },
                     { 
                         success: function(pushResponse) {
                            console.log("Sent push:", pushResponse);
                         }
                     });
                     push.wns.sendRaw(message.channel, JSON.stringify({ foo: 1, bar: 2 }));
                }
            } else {
                console.log('No recoreds are found.');
            }
        }
    });
    console.log('SQL is executed');
}