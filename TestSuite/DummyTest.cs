namespace TestSuite;

public class DummyTest {
    [Fact]
    public void Multiply_ShouldPass() {
        Assert.Equal(6, Multiply(2, 3));
    }

    public int Multiply(int x, int y) {
        return x * y;
    }
}