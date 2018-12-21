let CalendarJs = {

    dayClick: function (cal, datetime, allDay, el) {

        alert(datetime);

        let clickedDate = new Date(datetime);
    
        let outParams = {           
            dateTime: clickedDate
        }
        console.log(outParams);        
        
        Ext.Ajax.request({
            url: "/Renting/SetTime/",
            method: 'post',
            params: outParams,
            success: function (result) {
                window.location.href = 'Renting/RentTransport/';
            }
        });

    },
}

