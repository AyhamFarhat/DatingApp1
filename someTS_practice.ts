// interface example
{
    interface Car{
        color: string;
        model: string;
        topSpeed?: number; // optional
    }

    const car1: Car = {
        color: 'blue',
        model: 'BMW'
    }
    const car2: Car = {
        color: 'red',
        model: 'Mercedes',
        topSpeed: 100
    }
}

// variable type example
{
    let data01 = 98;
    //data01 = 'y'; wrong type , to avoid this:
    
    let data02: number | string = 97;
    data02 = 'y'; // correct
}

// function type example
{
    const multiply = (a: number, b: number) => {
        return a * b;
    }
}

// array type example
{
    let array: number[] = [1,2,3];
    let array2: Array<number> = [1,2,3];
}

// object type example
{
    let obj: {name: string, age: number} = {
        name: 'John',
        age: 30
    }
}

// custom type example
{
    type User = {name: string, age: number};
    let user: User = {
        name: 'John',
        age: 30
    }
}

// union type example
{
    let username: string | number = 'John';
    username = 30;
}

// type alias example
{
    type ID = string;
    type PopularTag = string;
    type MaybePopularTag = PopularTag | null;

    interface UserInterface {
        id: ID;
        name: string;
    }

    const popularTags: PopularTag[] = ['dragon', 'coffee'];
}

// function type alias example
{
    type StringGenerator = () => string;
    const generateMessage: StringGenerator = () => 'Hello, World';
}

// interface example
{
    interface UserInterface {
        name: string;
        age?: number;
        getMessage(): string;
    }

    const user: UserInterface = {
        name: 'John',
        age: 30,
        getMessage() {
            return 'Hello ' + name;
        }
    }
}


export interface User {
    username: string;
    token: string;
}
