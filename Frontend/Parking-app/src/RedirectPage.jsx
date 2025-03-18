import { useState, useEffect } from "react";

export default function RedirectPage() {
  const [cars, setCars] = useState([]);
  const [selectedCar, setSelectedCar] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [newCar, setNewCar] = useState("");

  useEffect(() => {
    fetch("/api/cars")
      .then((res) => res.json())
      .then((data) => setCars(data));
  }, []);

  const registerCar = () => {
    fetch("/api/cars", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name: newCar }),
    }).then(() => {
      setCars([...cars, { name: newCar, cost: 0 }]);
      setNewCar("");
    });
  };

  const selectCar = (car) => {
    setSelectedCar(car);
    setIsConfirmOpen(true);
  };

  const confirmSelection = () => {
    console.log("Car selected:", selectedCar);
    setIsConfirmOpen(false);
  };

  return (
    <div className="container">
      <h1 className="title">Welcome! Register or Select a Car</h1>
      <div className="mb-4">
        <input
          type="text"
          className="input"
          placeholder="Enter car name"
          value={newCar}
          onChange={(e) => setNewCar(e.target.value)}
        />
        <button className="button" onClick={registerCar}>Register Car</button>
      </div>
      <div className="car-list">
        {cars.map((car, index) => (
          <div key={index} onClick={() => selectCar(car)} className="card">
            <p className="text-lg">{car.name}</p>
            <p className="text-sm">Cost: ${car.cost}</p>
          </div>
        ))}
      </div>
      {isConfirmOpen && (
        <div className="dialog">
          <p>Are you sure you want to use {selectedCar?.name}?</p>
          <button className="button" onClick={() => setIsConfirmOpen(false)}>Cancel</button>
          <button className="button" onClick={confirmSelection}>Confirm</button>
        </div>
      )}
    </div>
  );
}

