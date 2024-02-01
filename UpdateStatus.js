function updateStatus(){
    var formContext = Xrm.Page;
   
    var _status = formContext.getAttribute("cr916_bookingstatuses").getValue();
 
    if(_status != 871210004){
        formContext.getAttribute("cr916_bookingstatuses").setValue(871210004);
    }
 }
   
 updateStatus();