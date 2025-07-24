package wharris.SWORD100CartServiceAPI;

import java.util.NoSuchElementException;
import java.util.UUID;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.HttpStatus;
import java.util.*;

//import java.time.LocalDate;

@RestController
@RequestMapping("/cart")
public class CartRestController {

    @Autowired
    private RedisTemplate<String, Cart> redisTemplate;

    @GetMapping(path = "/test")
    @ResponseStatus(code = HttpStatus.OK)
    public String test() {
        return "hello from cart service api";
    }

    @PostMapping(path = "/{cartId}")
    @ResponseStatus(code = HttpStatus.CREATED)
    public Cart addSingleBookToBasket(@PathVariable String cartId, @RequestBody Sword sword) {
        Cart cart = null;

        if ("new".equals(cartId)) {
            cart = new Cart(UUID.randomUUID().toString());
        } else {
            cart = redisTemplate.opsForValue().get(cartId);
        }

        validateCartExists(cart, cartId);
        cart.getSwords().add(sword);
        redisTemplate.opsForValue().set(cart.getCartGuid(), cart);

        return cart;
    }

    @PostMapping(path = "/addswords/{cartId}")
    @ResponseStatus(code = HttpStatus.CREATED)
    public Cart addListOfSwordsToCart(@PathVariable String cartId, @RequestBody List<Sword> swords) {
        Cart cart = null;

        if ("new".equals(cartId)) {
            cart = new Cart(UUID.randomUUID().toString());
        } else {
            cart = redisTemplate.opsForValue().get(cartId);
        }

        validateCartExists(cart, cartId);

        for (Sword sword : swords) {
            cart.getSwords().add(sword);
        }

        redisTemplate.opsForValue().set(cart.getCartGuid(), cart);

        return cart;
    }

    @GetMapping(path = "/{cartId}")
    @ResponseStatus(code = HttpStatus.OK)
    public Cart getCart(@PathVariable String cartId) {
        Cart cart = redisTemplate.opsForValue().get(cartId);
        return cart;
    }

    @DeleteMapping(path = "/{cartId}/{swordUUID}")
    @ResponseStatus(code = HttpStatus.OK)
    public void deleteSwordFromcart(@PathVariable String cartId, @PathVariable UUID swordUUID) {

        Cart cart = redisTemplate.opsForValue().get(cartId);
        validateCartExists(cart, cartId);
        cart.setSwords(cart.getSwords().stream().filter(b -> !b.getSwordGuid().equals(swordUUID)).collect(Collectors.toList()));
        redisTemplate.opsForValue().set(cartId, cart);
    }

    @DeleteMapping(path = "/{cartId}")
    @ResponseStatus(code = HttpStatus.OK)
    public void deleteCart(@PathVariable String cartId) {

        redisTemplate.delete(cartId);
    }

    private void validateCartExists(Cart cart, String cartId) {
        if (cart == null) {
            throw new NoSuchElementException("No such cart " + cartId);
        }
    }

}
