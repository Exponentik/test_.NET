import axios from "axios";

export const fetchUnits = async () =>{
    try{
    var response = await axios.get("http://localhost:5195/api/units");
    return response.data;
    } catch(e)
    {
        console.error(e);
    }
};
