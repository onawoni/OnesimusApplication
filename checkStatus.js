function checkStatus(){
    var formContext = Xrm.Page;
   
    var _value = false;
    var _status = formContext.getAttribute("cr916_bookingstatuses").getValue();
 
    if(_status == 871210004){
        _value = true;
    }
    return _value
 }
   
 checkStatus();