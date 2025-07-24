package wharris.SWORD100CartServiceAPI;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;
import org.springframework.data.annotation.Id;

public class Cart implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    private String cartGuid;

    private List<Sword> swords = new ArrayList<>();

    public Cart() {
    }

    public Cart(String id) {
        this.setCartGuid(id);
    }

    public String getCartGuid() {
        return cartGuid;
    }

    public void setCartGuid(String cartId) {
        this.cartGuid = cartId;
    }

    public List<Sword> getSwords() {
        return swords;
    }

    public void setSwords(List<Sword> swords) {
        this.swords = swords;
    }
}
