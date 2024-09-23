import React, { useEffect, useState } from "react";
import { fetchUnits } from "../services/UnitService";
import UnitItem from "./UnitItem";

const UnitList = () => {
    const [units, setUnits] = useState([]);

    const fetchData = async () => {
        const fetchedUnits = await fetchUnits();
        // Ensure that fetchedUnits is an array
        setUnits(fetchedUnits || []);
    };

    useEffect(() => {
        fetchData(); // Initial call to load data

        const interval = setInterval(fetchData, 3000); // Update data every 3 seconds

        return () => clearInterval(interval); // Clear interval on component unmount
    }, []);

    if (!Array.isArray(units) || units.length === 0) {
        return <h1 style={{ textAlign: 'center' }}>Департаменты не найдены</h1>;
    }

    // Function to build tree
    const buildTree = (units) => {
        const map = {};
        units.forEach(unit => {
            map[unit.id] = { ...unit, children: [] };
        });
        const tree = [];
        units.forEach(unit => {
            if (unit.parentId) {
                map[unit.parentId].children.push(map[unit.id]);
            } else {
                tree.push(map[unit.id]);
            }
        });
        return tree;
    };

    // Recursive function to render tree
    const renderTree = (nodes) => {
        return nodes.map(node => (
            <div key={node.id} style={{ marginLeft: '20px' }}>
                <UnitItem className="unit" unit={node} />
                {node.children.length > 0 && renderTree(node.children)}
            </div>
        ));
    };

    const unitTree = buildTree(units);

    return (
        <div>
            {renderTree(unitTree)}
        </div>
    );
};

export default UnitList;
