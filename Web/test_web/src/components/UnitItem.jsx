const UnitItem = ({unit})=>{
    return(
        
            <div className="unit">
                <strong>{unit.name}</strong>
                <h5>{unit.status}</h5>
                
                </div>
    );
}

export default  UnitItem;