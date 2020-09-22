
export interface KeyValuePair {
    id: number;
    name: string;
}

export interface Contact {
    name: string;
    phone: string;
    email: string;
}


export interface Vehicle {
    id: number;
    make: KeyValuePair;
    model: KeyValuePair;
    contact: Contact
    features: KeyValuePair[];
    isRegistered: boolean;
    lastUpdate: string
}

export interface SaveVehicle {
    id: number;
    makeId: number;
    modelId: number;
    features: number[];
    isRegistered: boolean;
    contact: Contact  
}