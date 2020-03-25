import { LinkedList } from "../linked-list";

describe("Linked list", () => {

    test("addLast", () => {
        const list = new LinkedList<IDataTest>();
        list.addLast(data1);
        expect(list.count).toBe(1);
        expect(list.last?.data).toMatchObject(data1);
        expect(list.first?.data).toMatchObject(data1);
        list.addLast(data2);
        expect(list.count).toBe(2);
        expect(list.last?.data).toMatchObject(data2);
        expect(list.first?.data).toMatchObject(data1);
        list.addLast(data3);
        expect(list.count).toBe(3);
        expect(list.last?.data).toMatchObject(data3);
        expect(list.first?.data).toMatchObject(data1);
    });

    test("addAt", () => {
        const list = new LinkedList<IDataTest>();
        list.addAt(data1, 0);
        check(list, 1, data1, data1);

        list.addAt(data2, 0);
        check(list, 2, data2, data1);

        list.addAt(data3, 2);
        check(list, 3, data2, data3);
        expect(list.elementAt(1).data).toMatchObject(data1);

        list.addAt(data4, 1);
        check(list, 4, data2, data3);
        expect(list.elementAt(1).data).toMatchObject(data4);
        expect(list.elementAt(2).data).toMatchObject(data1);
    });

    test("remove", () => {
        const list = new LinkedList<IDataTest>();
        list.addLast(data1);
        list.addLast(data2);
        list.addLast(data3);
        list.addLast(data4);
        list.addLast(data5);

        list.remove(1);
        expect(list.count).toBe(4);
        expect(list.elementAt(0).data).toMatchObject(data1);
        expect(list.elementAt(1).data).toMatchObject(data3);
        expect(list.elementAt(2).data).toMatchObject(data4);
        expect(list.elementAt(3).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data1);
        expect(list.last?.data).toMatchObject(data5);

        list.remove(0);
        expect(list.count).toBe(3);
        expect(list.elementAt(0).data).toMatchObject(data3);
        expect(list.elementAt(1).data).toMatchObject(data4);
        expect(list.elementAt(2).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data3);
        expect(list.last?.data).toMatchObject(data5);

        list.remove(2);
        expect(list.count).toBe(2);
        expect(list.elementAt(0).data).toMatchObject(data3);
        expect(list.elementAt(1).data).toMatchObject(data4);
        expect(list.first?.data).toMatchObject(data3);
        expect(list.last?.data).toMatchObject(data4);
    });

    test("move", () => {
        const list = new LinkedList<IDataTest>();
        list.addLast(data1);
        list.addLast(data2);
        list.addLast(data3);
        list.addLast(data4);
        list.addLast(data5);

        list.move(1, 3);

        expect(list.elementAt(0).data).toMatchObject(data1);
        expect(list.elementAt(1).data).toMatchObject(data3);
        expect(list.elementAt(2).data).toMatchObject(data4);
        expect(list.elementAt(3).data).toMatchObject(data2);
        expect(list.elementAt(4).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data1);
        expect(list.last?.data).toMatchObject(data5);

        list.move(3, 1);

        expect(list.elementAt(0).data).toMatchObject(data1);
        expect(list.elementAt(1).data).toMatchObject(data2);
        expect(list.elementAt(2).data).toMatchObject(data3);
        expect(list.elementAt(3).data).toMatchObject(data4);
        expect(list.elementAt(4).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data1);
        expect(list.last?.data).toMatchObject(data5);

        list.move(1, 2);

        expect(list.elementAt(0).data).toMatchObject(data1);
        expect(list.elementAt(1).data).toMatchObject(data3);
        expect(list.elementAt(2).data).toMatchObject(data2);
        expect(list.elementAt(3).data).toMatchObject(data4);
        expect(list.elementAt(4).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data1);
        expect(list.last?.data).toMatchObject(data5);

        list.move(0, 4);

        expect(list.elementAt(0).data).toMatchObject(data3);
        expect(list.elementAt(1).data).toMatchObject(data2);
        expect(list.elementAt(2).data).toMatchObject(data4);
        expect(list.elementAt(3).data).toMatchObject(data5);
        expect(list.elementAt(4).data).toMatchObject(data1);
        expect(list.first?.data).toMatchObject(data3);
        expect(list.last?.data).toMatchObject(data1);

        list.move(4, 0);

        expect(list.elementAt(0).data).toMatchObject(data1);
        expect(list.elementAt(1).data).toMatchObject(data3);
        expect(list.elementAt(2).data).toMatchObject(data2);
        expect(list.elementAt(3).data).toMatchObject(data4);
        expect(list.elementAt(4).data).toMatchObject(data5);
        expect(list.first?.data).toMatchObject(data1);
        expect(list.last?.data).toMatchObject(data5);
    });
});

function check(list: LinkedList<IDataTest>, count: number, first: IDataTest, last: IDataTest) {
    expect(list.count).toBe(count);
    expect(list.first?.data).toMatchObject(first);
    expect(list.last?.data).toMatchObject(last);
}

interface IDataTest {
    field1: string;
    field2: string;
}

const data1: IDataTest = { field1: "f11", field2: "f12" };
const data2: IDataTest = { field1: "f21", field2: "f22" };
const data3: IDataTest = { field1: "f31", field2: "f32" };
const data4: IDataTest = { field1: "f41", field2: "f42" };
const data5: IDataTest = { field1: "f51", field2: "f52" };
